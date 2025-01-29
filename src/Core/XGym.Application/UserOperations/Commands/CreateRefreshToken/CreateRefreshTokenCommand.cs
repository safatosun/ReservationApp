using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using XGym.Application.Common.Shared;
using XGym.Application.UserOperations.Commands.CreateToken;
using XGym.Domain.Entities;

namespace XGym.Application.UserOperations.Commands.CreateRefreshToken
{
    public record CreateRefreshTokenCommand : IRequest<Response<TokenDto>>
    {
        public string AccessToken { get; init; }
        public string RefreshToken { get; init; }
    }

    public class CreateRefreshTokenCommandHandler : IRequestHandler<CreateRefreshTokenCommand, Response<TokenDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public CreateRefreshTokenCommandHandler(IConfiguration configuration, UserManager<User> userManager, IMediator mediator)
        {
            _configuration = configuration;
            _userManager = userManager;
            _mediator = mediator;
        }

        public async Task<Response<TokenDto>> Handle(CreateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var principal = GetPrincipalFromExpiredToken(request.AccessToken);
           
            var user = await _userManager.FindByIdAsync(principal.FindFirstValue(ClaimTypes.NameIdentifier));

            if (user is null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpireTime <= DateTime.Now)
            {
                return new Response<TokenDto>(ResponseCode.Fail, $"Failed to update the token");
            }

            var tokenRequest = new CreateTokenCommand { User = user };

            return await _mediator.Send(tokenRequest);
        }
    

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["secretKey"];

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["validIssuer"],
                ValidAudience = jwtSettings["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;


           if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token.");
            }

            return principal;

        }

    }
}
