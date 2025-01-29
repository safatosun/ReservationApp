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
using XGym.Application.ReservationOperations.Queries.GetReservationsByUserIdWithPagination;
using XGym.Domain.Entities;

namespace XGym.Application.ReservationOperations.Queries.GetReservationsWithPagination
{
    public record GetReservationsWithPaginationQuery : IRequest<(Response<IEnumerable<ReservationsDto>>, MetaData)>
    {
        public ReservationRequestParameters requestParameters { get; set; }
    }

    public class GetReservationsWithPaginationQueryHandler : IRequestHandler<GetReservationsWithPaginationQuery, (Response<IEnumerable<ReservationsDto>>, MetaData)>
    {
        private readonly IXGymDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ITenantUtility _tenantUtility;

        public GetReservationsWithPaginationQueryHandler(IXGymDbContext dbContext, IMapper mapper, UserManager<User> userManager, ITenantUtility tenantUtility)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
            _tenantUtility = tenantUtility;
        }

        public async Task<(Response<IEnumerable<ReservationsDto>>, MetaData)> Handle(GetReservationsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var user = await _tenantUtility.GetUserAsync();

            var isAdminUser = await _userManager.IsInRoleAsync(user, "Admin");

            if (!isAdminUser)
                return (new Response<IEnumerable<ReservationsDto>>(ResponseCode.Forbidden, "Access denied: You do not have sufficient permissions."),null);

            var reservationsQuery = _dbContext.Reservations.Include(r => r.User).Where(r=>r.StartTime.Date >= request.requestParameters.StartDate.Date && r.EndTime.Date <= request.requestParameters.EndDate.Date).Skip(((int)request.requestParameters.PageNumber - 1) * (int)request.requestParameters.PageSize).Take((int)request.requestParameters.PageSize);

            var reservations = await reservationsQuery.ToListAsync();

            if (!reservations.Any())
                return (new Response<IEnumerable<ReservationsDto>>(ResponseCode.NoContent, "No reservations found."), null);

            var reservationsDto = _mapper.Map<IEnumerable<ReservationsDto>>(reservations);           

            var metaData = new MetaData()
            {
                TotalCount = (uint)reservations.Count(),
                PageSize = request.requestParameters.PageSize,
                CurrentPage = request.requestParameters.PageNumber,
                TotalPage = (uint)Math.Ceiling(reservations.Count() / (double)request.requestParameters.PageSize)
            };
            
            var response = new Response<IEnumerable<ReservationsDto>>(ResponseCode.Success, reservationsDto, "Users reservations retrieved successfully.");
            
            return (response, metaData);
        }
    }



}
