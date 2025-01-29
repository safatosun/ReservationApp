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
using XGym.Application.UserDetailOperations.Queries.GetUserDetailByUserId;
using XGym.Application.UserDetailOperations.Queries.GetUsersDetailsWithPagination;
using XGym.Domain.Entities;

namespace XGym.Application.UserOperations.Queries.GetUsersWithPagination
{
    public record GetUsersWithPaginationQuery : IRequest<(Response<IEnumerable<UsersDto>>, MetaData)>
    {
        public RequestParameters requestParameters { get; set; }
    }

    public class GetUsersWithPaginationQueryHandler : IRequestHandler<GetUsersWithPaginationQuery, (Response<IEnumerable<UsersDto>>, MetaData)>
    {
        private readonly IXGymDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ITenantUtility _tenantUtility;

        public GetUsersWithPaginationQueryHandler(IXGymDbContext dbContext, IMapper mapper, UserManager<User> userManager, ITenantUtility tenantUtility)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
            _tenantUtility = tenantUtility;
        }

        public async  Task<(Response<IEnumerable<UsersDto>>, MetaData)> Handle(GetUsersWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var user = await _tenantUtility.GetUserAsync();

            var isAdminUser = await _userManager.IsInRoleAsync(user, "Admin");

            if (!isAdminUser)
                return (new Response<IEnumerable<UsersDto>>(ResponseCode.Forbidden, "Access denied: You do not have sufficient permissions."), null);

            var usersQuery = _dbContext.Users.Skip(((int)request.requestParameters.PageNumber - 1) * (int)request.requestParameters.PageSize).Take((int)request.requestParameters.PageSize);

            var users = await usersQuery.ToListAsync();

            if (!users.Any())
                return (new Response<IEnumerable<UsersDto>>(ResponseCode.NoContent, "No users to display."), null);

            var usersDto = _mapper.Map<IEnumerable<UsersDto>>(users);

            var metaData = new MetaData()
            {
                TotalCount = (uint)users.Count(),
                PageSize = request.requestParameters.PageSize,
                CurrentPage = request.requestParameters.PageNumber,
                TotalPage = (uint)Math.Ceiling(users.Count() / (double)request.requestParameters.PageSize)
            };

            var response = new Response<IEnumerable<UsersDto>>(ResponseCode.Success, usersDto, "Users retrieved successfully.");
            return (response, metaData);
        }
    }

}
