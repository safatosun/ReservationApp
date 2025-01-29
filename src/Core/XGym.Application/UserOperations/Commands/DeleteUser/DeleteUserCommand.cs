
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

namespace XGym.Application.UserOperations.Commands.DeleteUser
{
    public record DeleteUserCommand : IRequest<Response>
    {
        public string Id { get; init; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Response>
    {
        private readonly UserManager<User> _userManager;
        private readonly ITenantUtility _tenantUtility;
        public DeleteUserCommandHandler(UserManager<User> userManager, ITenantUtility tenantUtility)
        {
            _userManager = userManager;
            _tenantUtility = tenantUtility;
        }

        public async Task<Response> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {

            var user = await _tenantUtility.GetUserAsync();
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            if (request.Id != null && request.Id != user.Id && !isAdmin)
            {
                return new Response(ResponseCode.Forbidden, "You do not have permission to delete this user.");
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                return new Response(ResponseCode.Fail, $"User deletion failed: {errorMessage}");
            }

            return new Response(ResponseCode.Success, "User has been successfully deleted.");
        }
    }

}
