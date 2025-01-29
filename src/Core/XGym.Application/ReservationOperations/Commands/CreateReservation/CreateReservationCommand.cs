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

namespace XGym.Application.ReservationOperations.Commands.CreateReservation
{
    public record CreateReservationCommand : IRequest<Response>
    {
        public DateTime StartTime { get; init; }
        public DateTime EndTime { get; init; }
        public string? SpecialRequest { get; init; }
    }

    public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, Response>
    {
        private readonly IXGymDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ITenantUtility _tenantUtility;

        public CreateReservationCommandHandler(IXGymDbContext dbContext, IMapper mapper, UserManager<User> userManager, ITenantUtility tenantUtility)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _tenantUtility = tenantUtility;
        }

        public async Task<Response> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {

            var userId = _tenantUtility.GetUserId();

            if (request.EndTime - request.StartTime < TimeSpan.FromHours(1))
                return new Response(ResponseCode.BadRequest, "Reservation duration must be at least 1 hour.");

            if (request.EndTime - request.StartTime > TimeSpan.FromHours(2))
                return new Response(ResponseCode.BadRequest, "Reservation duration cannot exceed 2 hours.");

            var checkReservations =  await _dbContext.Reservations.Where(x => x.UserId == userId &&
                (
                    (x.StartTime >= request.StartTime && x.StartTime <= request.EndTime) ||

                    (x.EndTime >= request.StartTime && x.EndTime <= request.EndTime) ||

                    (x.StartTime <= request.StartTime && x.EndTime >= request.EndTime)
                )).AnyAsync();

            if (checkReservations)
                return new Response(ResponseCode.BadRequest, "The reservation is definitely conflicting with existing reservations, check it.");

            var reservation = _mapper.Map<Reservation>(request);
            reservation.UserId = userId;
            reservation.Status = ReservationStatus.Pending;

            await _dbContext.Reservations.AddAsync(reservation);
            var result = await _dbContext.SaveChangesAsync();

            if (result <= 0)
                return new Response(ResponseCode.Fail, "An error occurred while adding the reservation.");

            return new Response(ResponseCode.Created, "Reservation has been successfully created.");
        }
    }

}
