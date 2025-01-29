using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XGym.Application.Common.Shared;
using XGym.Domain.Entities;

namespace XGym.Application.UserOperations.Commands.UpdateUserPassword
{
    public record UpdateUserPasswordCommand : IRequest<Response>
    {
        public string Email { get; init; }
        public string CurrentPassword { get; init; }
        public string NewPassword { get; init; }
        public string ConfirmNewPassword { get; init; }

    }

    public record UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand, Response>
    {
        private readonly UserManager<User> _userManager;

        public UpdateUserPasswordCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new Response(ResponseCode.BadRequest, "User not found.");
            }

            if (request.NewPassword != request.ConfirmNewPassword)
            {
                return new Response(ResponseCode.BadRequest, "New password and confirmation password do not match.");
            }

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (!result.Succeeded)
            {
                var errorMessage = result.Errors.FirstOrDefault()?.Description ?? "Password change failed.";
                return new Response(ResponseCode.Fail, errorMessage);
            }

            return new Response(ResponseCode.Success, "Password changed successfully.");
        }
    }

}
