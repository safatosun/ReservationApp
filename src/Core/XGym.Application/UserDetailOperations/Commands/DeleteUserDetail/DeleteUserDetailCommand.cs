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

namespace XGym.Application.UserDetailOperations.Commands.DeleteUserDetail
{
    public record DeleteUserDetailCommand : IRequest<Response>
    {

    }

    public class DeleteUserDetailCommandHandler : IRequestHandler<DeleteUserDetailCommand, Response>
    {
        private readonly IXGymDbContext _dbContext;
        private readonly ITenantUtility _tenantUtility;

        public DeleteUserDetailCommandHandler(IXGymDbContext dbContext, ITenantUtility tenantUtility)
        {
            _dbContext = dbContext;
            _tenantUtility = tenantUtility;
        }

        public async Task<Response> Handle(DeleteUserDetailCommand request, CancellationToken cancellationToken)
        {
            var userId = _tenantUtility.GetUserId();

            var userDetail = await _dbContext.UserDetails.Where(ud => ud.UserId == userId).FirstOrDefaultAsync();

            if (userDetail == null)
                return new Response(ResponseCode.NotFound, "No user detail found with the given User.");

            _dbContext.UserDetails.Remove(userDetail);

            var result = await _dbContext.SaveChangesAsync();

            if (result <= 0)
                return new Response(ResponseCode.Fail, "An error occurred while deleting the user detail.");

            return new Response(ResponseCode.Created, "User detail has been successfully deleted.");

        }
    }

}
