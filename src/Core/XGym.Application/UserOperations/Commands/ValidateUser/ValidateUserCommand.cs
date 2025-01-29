using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XGym.Application.Common.Shared;
using XGym.Application.UserOperations.Commands.CreateToken;
using XGym.Domain.Entities;

namespace XGym.Application.UserOperations.Commands.ValidateUser
{
    public record ValidateUserCommand : IRequest<Response<TokenDto>>
    {
        public string UserEmail { get; init; }
        public string Password { get; init; }
    }

    public class ValidateUserCommandHandler : IRequestHandler<ValidateUserCommand, Response<TokenDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMediator _mediator;

        public ValidateUserCommandHandler(UserManager<User> userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        public async Task<Response<TokenDto>> Handle(ValidateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.UserEmail);

            var result = (user != null && await _userManager.CheckPasswordAsync(user, request.Password)); 

            if (!result)
                return new Response<TokenDto>(ResponseCode.UnAuthorized, "The user email or password is incorrect.");

            var tokenRequest = new CreateTokenCommand { User = user };

            return await _mediator.Send(tokenRequest);
        }
    }

}
