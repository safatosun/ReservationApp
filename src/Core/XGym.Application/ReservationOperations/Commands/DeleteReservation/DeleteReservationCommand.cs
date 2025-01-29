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

namespace XGym.Application.ReservationOperations.Commands.DeleteReservation
{
    public record DeleteReservationCommand : IRequest<Response>
    {
        public int Id { get; set; }
    }

    public class DeleteReservationCommandHandler : IRequestHandler<DeleteReservationCommand, Response>
    { 
        private readonly IXGymDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly ITenantUtility _tenantUtility;

        public DeleteReservationCommandHandler(IXGymDbContext dbContext, UserManager<User> userManager, ITenantUtility tenantUtility)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _tenantUtility = tenantUtility;
        }

        public async Task<Response> Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
        {
            var user = await _tenantUtility.GetUserAsync();
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            var reservation = await _dbContext.Reservations.Where(r => r.Id == request.Id).FirstOrDefaultAsync();

            if (reservation == null)
                return new Response(ResponseCode.NotFound, "Reservation not found.");

            if (!isAdmin && reservation.UserId != user.Id)
                return new Response(ResponseCode.Forbidden, "You cannot delete this reservation.");

            if (!isAdmin &&  reservation.Status != ReservationStatus.Pending)
                return new Response(ResponseCode.BadRequest, "Only pending reservations can be deleted.");

            _dbContext.Reservations.Remove(reservation);
            var result = await _dbContext.SaveChangesAsync();

            if (result <= 0)
                return new Response(ResponseCode.Fail, "An error occurred while deleting the reservation.");

            return new Response(ResponseCode.Success, "Reservation has been successfully deleted.");

        }
    }

}
