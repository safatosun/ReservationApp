using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XGym.Application.Common.Shared;
using XGym.Application.Common.Utility;
using XGym.Domain.Entities;

namespace XGym.Application.UserOperations.Queries.GetUserById
{
    public record GetUserByUserIdQuery : IRequest<Response<UserViewModel>>
    {

    }

    public class GetUserByUserIdQueryHandler : IRequestHandler<GetUserByUserIdQuery, Response<UserViewModel>>
    {
        private readonly IMapper _mapper;
        private readonly ITenantUtility _tenantUtility;

        public GetUserByUserIdQueryHandler(ITenantUtility tenantUtility, IMapper mapper)
        {
            _tenantUtility = tenantUtility;
            _mapper = mapper;
        }

        public async Task<Response<UserViewModel>> Handle(GetUserByUserIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _tenantUtility.GetUserAsync();
            
            var userViewModel = _mapper.Map<UserViewModel>(user);

            return new Response<UserViewModel>(ResponseCode.Success,userViewModel, "User retrieved successfully.");
        }
    }

}
