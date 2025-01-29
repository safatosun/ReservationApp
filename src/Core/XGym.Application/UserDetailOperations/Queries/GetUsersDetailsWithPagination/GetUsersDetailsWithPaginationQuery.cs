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

namespace XGym.Application.UserDetailOperations.Queries.GetUsersDetailsWithPagination
{
    public record GetUsersDetailsWithPaginationQuery : IRequest<(Response<IEnumerable<UsersDetailsDto>>, MetaData)>
    {
        public RequestParameters requestParameters { get; set; }
    }

    public class GetUsersDetailsWithPaginationQueryHandler : IRequestHandler<GetUsersDetailsWithPaginationQuery, (Response<IEnumerable<UsersDetailsDto>>, MetaData)>
    {
        private readonly IXGymDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ITenantUtility _tenantUtility;

        public GetUsersDetailsWithPaginationQueryHandler(IXGymDbContext dbContext, IMapper mapper, UserManager<User> userManager, ITenantUtility tenantUtility)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
            _tenantUtility = tenantUtility;
        }

        public async Task<(Response<IEnumerable<UsersDetailsDto>>, MetaData)> Handle(GetUsersDetailsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var user = await _tenantUtility.GetUserAsync();
            var isAdminUser = await _userManager.IsInRoleAsync(user, "Admin");

            if (!isAdminUser)
                return (new Response<IEnumerable<UsersDetailsDto>>(ResponseCode.Forbidden, "Access denied: You do not have sufficient permissions."),null);

            var userDetailsQuery = _dbContext.UserDetails.Include(ud=>ud.User).Skip(((int)request.requestParameters.PageNumber - 1) * (int)request.requestParameters.PageSize).Take((int)request.requestParameters.PageSize);

            var userDetails = await userDetailsQuery.ToListAsync();

            if (!userDetails.Any())
                return (new Response<IEnumerable<UsersDetailsDto>>(ResponseCode.NoContent, "No users detail to display."), null);

            var userDetailsDto = _mapper.Map<IEnumerable<UsersDetailsDto>>(userDetails);

            var metaData = new MetaData()
            {
                TotalCount = (uint)userDetails.Count(),
                PageSize = request.requestParameters.PageSize,
                CurrentPage = request.requestParameters.PageNumber,
                TotalPage = (uint)Math.Ceiling(userDetails.Count() / (double)request.requestParameters.PageSize)
            };

            var response = new Response<IEnumerable<UsersDetailsDto>>(ResponseCode.Success, userDetailsDto, "Users details retrieved successfully.");

            return (response,metaData);
        }


    }
}
