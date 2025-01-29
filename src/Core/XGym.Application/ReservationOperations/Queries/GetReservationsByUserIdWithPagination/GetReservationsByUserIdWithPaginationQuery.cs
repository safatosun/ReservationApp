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
using XGym.Application.Common.Parameter;
using XGym.Application.Common.Shared;
using XGym.Application.Common.Utility;
using XGym.Application.ReservationOperations.Queries.GetReservationsWithPagination;
using XGym.Domain.Entities;

namespace XGym.Application.ReservationOperations.Queries.GetReservationsByUserIdWithPagination
{
    
    public record GetReservationsByUserIdWithPaginationQuery : IRequest<(Response<IEnumerable<UserReservationsDto>>, MetaData)>
    {
        public ReservationRequestParameters requestParameters { get; set; }
    }

    public class GetReservationsByUserIdWithPaginationQueryHandler : IRequestHandler<GetReservationsByUserIdWithPaginationQuery, (Response<IEnumerable<UserReservationsDto>>, MetaData)>
    {
        private readonly IXGymDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ITenantUtility _tenantUtility;

        public GetReservationsByUserIdWithPaginationQueryHandler(IXGymDbContext dbContext, IMapper mapper, ITenantUtility tenantUtility)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _tenantUtility = tenantUtility;
        }

        public async Task<(Response<IEnumerable<UserReservationsDto>>, MetaData)> Handle(GetReservationsByUserIdWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var userId = _tenantUtility.GetUserId();

            var reservationsQuery = _dbContext.Reservations.Include(r => r.User).Where(r=>r.UserId == userId && r.StartTime.Date >= request.requestParameters.StartDate.Date && r.EndTime.Date <= request.requestParameters.EndDate.Date)
                .Skip(((int)request.requestParameters.PageNumber - 1) * (int)request.requestParameters.PageSize).Take((int)request.requestParameters.PageSize);

            var reservations = await reservationsQuery.ToListAsync();

            if (!reservations.Any())
                return (new Response<IEnumerable<UserReservationsDto>>(ResponseCode.NoContent, "No reservations found."), null);

            var userReservationsDto = _mapper.Map<IEnumerable<UserReservationsDto>>(reservations);

            var metaData = new MetaData()
            {
                TotalCount = (uint)reservations.Count(),
                PageSize = request.requestParameters.PageSize,
                CurrentPage = request.requestParameters.PageNumber,
                TotalPage = (uint)Math.Ceiling(reservations.Count() / (double)request.requestParameters.PageSize)
            };

            var response = new Response<IEnumerable<UserReservationsDto>>(ResponseCode.Success, userReservationsDto, "User reservations retrieved successfully.");

            return (response, metaData);
        }
    }


}
