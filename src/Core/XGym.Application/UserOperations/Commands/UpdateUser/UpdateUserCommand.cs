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

namespace XGym.Application.UserOperations.Commands.UpdateUser
{
    public record UpdateUserCommand : IRequest<Response>
    {
        public string Name { get; init; }
        public string Surname { get; init; }
        public string Email { get; init; }
        public string PhoneNumber { get; init; }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Response>
    { 
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ITenantUtility _tenantUtility;

        public UpdateUserCommandHandler(UserManager<User> userManager, IMapper mapper, ITenantUtility tenantUtility)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tenantUtility = tenantUtility;
        }

        public async Task<Response> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {

            var user = await _tenantUtility.GetUserAsync();

            if (user == null)
                return new Response(ResponseCode.NotFound, $"User cannot found.");

            _mapper.Map(request, user);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                return new Response(ResponseCode.BadRequest, $"User update failed: {errorMessage}");
            }

            return new Response(ResponseCode.Success, "User has been successfully updated.");

        }
    }

}
