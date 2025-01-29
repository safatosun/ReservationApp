using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using XGym.Application.Common.Interfaces;
using XGym.Application.Common.Shared;
using XGym.Domain.Entities;

namespace XGym.Application.UserOperations.Commands.CreateToken
{
    public record CreateTokenCommand : IRequest<Response<TokenDto>>
    {
        public User User { get; set; }
    }

    public class CreateTokenCommandHandler : IRequestHandler<CreateTokenCommand, Response<TokenDto>>
    {
        private readonly IXGymDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private User _user;

        public CreateTokenCommandHandler(IXGymDbContext dbContext, IMapper mapper, UserManager<User> userManager, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<Response<TokenDto>> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
        {
            _user = request.User;

            var signinCredentials = GetSiginCredentials();
            var claims = await GetAndCreateClaims();
            var tokenOptions = GenerateTokenOptions(signinCredentials, claims);

            var refreshToken = GenerateRefreshToken();
            
            _user.RefreshToken = refreshToken;
            _user.RefreshTokenExpireTime = DateTime.Now.AddMinutes(15);

            var userUpdateResult = await _userManager.UpdateAsync(_user);

            if (!userUpdateResult.Succeeded)
            {
                var errorMessage = string.Join(", ", userUpdateResult.Errors.Select(e => e.Description));
                return new Response<TokenDto>(ResponseCode.Fail, $"Failed to update user with refresh token: {errorMessage}");
            }

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            var token = new TokenDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };

            return new Response<TokenDto>(ResponseCode.Success, token, "Token successfully created.");

        }

        private SigningCredentials GetSiginCredentials()
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["secretKey"]);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetAndCreateClaims()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,_user.Id.ToString())
            };  

            var roles = await _userManager.GetRolesAsync(_user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signinCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var tokenOptions = new JwtSecurityToken(
                issuer: jwtSettings["validIssuer"],
                audience: jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
                signingCredentials: signinCredentials);

            return tokenOptions;

        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }

        }


    }

}
