using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XGym.Application.Common.Interfaces;
using XGym.Application.Common.Shared;
using XGym.Application.Common.Utility;
using XGym.Domain.Entities;
using XGym.Domain.Enums;

namespace XGym.Application.ReservationOperations.Commands.UpdateReservation
{
    public record UpdateReservationCommand : IRequest<Response>
    {
        public int Id { get; set; }
        public DateTime StartTime { get; init; }
        public DateTime EndTime { get; init; }
        public ReservationStatus Status { get; set; }
        public string? SpecialRequest { get; init; }
    }

    public class UpdateReservationCommandHandler : IRequestHandler<UpdateReservationCommand, Response>
    {
        private readonly IXGymDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ITenantUtility _tenantUtility;

        public UpdateReservationCommandHandler(IXGymDbContext dbContext, IMapper mapper, UserManager<User> userManager, ITenantUtility tenantUtility)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
            _tenantUtility = tenantUtility;
        }

        public async Task<Response> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
        {

            if (request.EndTime - request.StartTime < TimeSpan.FromHours(1))
                return new Response(ResponseCode.BadRequest, "Reservation duration must be at least 1 hour.");

            if (request.EndTime - request.StartTime > TimeSpan.FromHours(2))
                return new Response(ResponseCode.BadRequest, "Reservation duration cannot exceed 2 hours.");

            var reservation = await _dbContext.Reservations.Where(r => r.Id == request.Id).FirstOrDefaultAsync();

            if (reservation == null)
                return new Response(ResponseCode.NotFound, "Reservation not found.");

            if (DateTime.Now >= reservation.StartTime)
                return new Response(ResponseCode.BadRequest, "The reservation start time has already passed. You can no longer update it.");


            var user = await _tenantUtility.GetUserAsync();
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            if (reservation.Status != ReservationStatus.Pending && !isAdmin)
                return new Response(ResponseCode.BadRequest, "You can only update reservations that are in the pending stage.");

            if (!isAdmin && reservation.UserId != user.Id)
                return new Response(ResponseCode.Forbidden, "You can only update your own reservation.");

            if (!isAdmin)
                request.Status = reservation.Status;

            reservation = _mapper.Map(request,reservation);

            _dbContext.Reservations.Update(reservation);

            var result = await _dbContext.SaveChangesAsync();

            if (result <= 0)
                return new Response(ResponseCode.Fail, "An error occurred while updating the reservation.");

            return new Response(ResponseCode.Created, "Reservation has been successfully updated.");
        }
    }
}
