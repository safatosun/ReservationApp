using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XGym.Application.Common.Shared;
using XGym.Domain.Entities;

namespace XGym.Application.UserOperations.Commands.CreateUser
{
    public record CreateUserCommand : IRequest<Response>
    {      
        public string Name { get; init; }
        public string Surname { get; init; }
        public string UserName { get; set; }
        public string Email { get; init; }
        public string PhoneNumber { get; init; }
        public string Password { get; init; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Response>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<Response> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request);

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                return new Response(ResponseCode.BadRequest, $"User creation failed: {errorMessage}");
            }

            var addRoleResult = await _userManager.AddToRoleAsync(user, "User");

            if (!addRoleResult.Succeeded)
            {
                var roleErrorMessage = string.Join(", ", addRoleResult.Errors.Select(e => e.Description));
                return new Response(ResponseCode.BadRequest, $"User role assignment failed: {roleErrorMessage}");
            }

            return new Response(ResponseCode.Created, "User created and role assigned successfully.");
        }

    }

}
