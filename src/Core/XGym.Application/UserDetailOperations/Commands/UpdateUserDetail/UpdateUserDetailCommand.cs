using AutoMapper;
using MediatR;
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

namespace XGym.Application.UserDetailOperations.Commands.UpdateUserDetail
{
    public record UpdateUserDetailCommand : IRequest<Response>
    {
        public decimal Shoulder { get; init; }
        public decimal Chest { get; init; }
        public decimal Arm { get; init; }
        public decimal Waist { get; init; }
        public decimal Hip { get; init; }
        public decimal Thigh { get; init; }
    }


    public class UpdateUserDetailCommandHandler : IRequestHandler<UpdateUserDetailCommand, Response>
    {
        private readonly IXGymDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ITenantUtility _tenantUtility;

        public UpdateUserDetailCommandHandler(IXGymDbContext dbContext, IMapper mapper, ITenantUtility tenantUtility)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _tenantUtility = tenantUtility;
        }

        public async Task<Response> Handle(UpdateUserDetailCommand request, CancellationToken cancellationToken)
        {
            var userId = _tenantUtility.GetUserId();

            var userDetail = await _dbContext.UserDetails.Where(ud=>ud.UserId == userId).FirstOrDefaultAsync();

            if (userDetail == null)
                return new Response(ResponseCode.NotFound, "No user detail found with the given User.");

            _mapper.Map(request,userDetail);

            _dbContext.UserDetails.Update(userDetail);
            var result = await _dbContext.SaveChangesAsync();

            if (result <= 0)
                return new Response(ResponseCode.Fail, "An error occurred while updating the user detail.");

            return new Response(ResponseCode.Created, "User detail has been successfully updated.");
        }
    }

}
