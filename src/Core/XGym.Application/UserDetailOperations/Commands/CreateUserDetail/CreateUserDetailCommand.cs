using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XGym.Application.Common.Interfaces;
using XGym.Application.Common.Shared;
using XGym.Application.Common.Utility;
using XGym.Domain.Entities;

namespace XGym.Application.UserDetailOperations.Commands.CreateUserDetail
{
    public record CreateUserDetailCommand : IRequest<Response>
    {
        public decimal Shoulder { get; init; }
        public decimal Chest { get; init; }
        public decimal Arm { get; init; }
        public decimal Waist { get; init; }
        public decimal Hip { get; init; }
        public decimal Thigh { get; init; }
    }

    public class CreateUserDetailCommandHandler : IRequestHandler<CreateUserDetailCommand, Response>
    {   
        private readonly IXGymDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ITenantUtility _tenantUtility;

        public CreateUserDetailCommandHandler(IXGymDbContext dbContext, IMapper mapper, ITenantUtility tenantUtility)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _tenantUtility = tenantUtility;
        }

        public async Task<Response> Handle(CreateUserDetailCommand request, CancellationToken cancellationToken)
        {
            var userId = _tenantUtility.GetUserId();

            var checkExist = _dbContext.UserDetails.Any(ud=>ud.UserId == userId);

            if (checkExist)
                return new Response(ResponseCode.BadRequest, "User detail already exist.");

            var userDetail = _mapper.Map<UserDetail>(request);
            userDetail.UserId = userId;

            await _dbContext.UserDetails.AddAsync(userDetail);
            var result = await _dbContext.SaveChangesAsync();

            if (result <= 0)
                return new Response(ResponseCode.Fail, "An error occurred while adding the user detail.");

            return new Response(ResponseCode.Created, "User detail has been successfully created.");

        }
    }


}
