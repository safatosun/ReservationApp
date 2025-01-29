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
using XGym.Application.UserDetailOperations.Queries.GetUsersDetailsWithPagination;
using XGym.Domain.Entities;

namespace XGym.Application.UserDetailOperations.Queries.GetUserDetailByUserId
{
    public record GetUserDetailByUserIdQuery : IRequest<Response<UserDetailDto>>
    {

    }

    public class GetUserDetailByUserIdQueryHandler : IRequestHandler<GetUserDetailByUserIdQuery, Response<UserDetailDto>>
    {
        private readonly IXGymDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ITenantUtility _tenantUtility;

        public GetUserDetailByUserIdQueryHandler(IXGymDbContext dbContext, IMapper mapper, ITenantUtility tenantUtility)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _tenantUtility = tenantUtility;
        }

        public async Task<Response<UserDetailDto>> Handle(GetUserDetailByUserIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _tenantUtility.GetUserId();

            var userDetail = await _dbContext.UserDetails.Include(ud => ud.User).Where(ud=>ud.UserId == userId).FirstOrDefaultAsync();

            if (userDetail == null)
                return new Response<UserDetailDto>(ResponseCode.NoContent, "No user detail found with the given User.");

            var userDetailDto = _mapper.Map<UserDetailDto>(userDetail);

            return new Response<UserDetailDto>(ResponseCode.Success,userDetailDto ,"User detail retrieved successfully.");
        }
    }

}
