using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XGym.Domain.Entities;

namespace XGym.Application.Common.Utility
{
    public interface ITenantUtility
    {
        string GetUserId();
        Task<User> GetUserAsync();
    }

    public class TenantUtility : ITenantUtility
    {
        private readonly HttpContext _httpContext;
        private readonly UserManager<User> _userManager;

        public TenantUtility(IHttpContextAccessor contextAccessor, UserManager<User> userManager)
        {
            if (contextAccessor.HttpContext != null)
            {
                _httpContext = contextAccessor.HttpContext;
            }

            _userManager = userManager;
        }

        public string GetUserId()
        {
            return GetUserIdFromClaimValue();
        }

        public async Task<User> GetUserAsync()
        {
            var userId = GetUserId();
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new Exception("User could not be found with the ID obtained from the token.");
            
            return user;
        }

        private string GetUserIdFromClaimValue()
        {
            var apiUserId = _httpContext.User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");


            if (apiUserId == null)
                throw new Exception("User ID claim not found in the user's claims.");

            if (string.IsNullOrEmpty(apiUserId.Value))
                throw new Exception("User ID claim value is empty.");

            return apiUserId.Value;
        }

    }
}
