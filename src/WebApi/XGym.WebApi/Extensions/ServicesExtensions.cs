using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using XGym.Domain.Entities;
using XGym.Persistence.Data.Context;

namespace XGym.WebApi.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection ConfigureCors(this IServiceCollection services)
        {            
            services.AddCors(options =>
            {             
                options.AddPolicy("XGymCorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("X-Pagination"));
            });
            return services;
        }

        public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<User, IdentityRole>(opts =>
            {
                opts.Password.RequireDigit = true;
                opts.Password.RequireLowercase = false; 
                opts.Password.RequireUppercase = false;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequiredLength = 6; 

                opts.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<XGymDbContext>() 
              .AddDefaultTokenProviders();
            return services;
        }

        public static IServiceCollection ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["secretKey"];

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;                    
            }).AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {   
                ValidateIssuer = true, 
                ValidateAudience = true, 
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["validIssuer"],               
                ValidAudience = jwtSettings["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            });

            return services;
        }

        public static IServiceCollection  ConfigureRateLimit(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>()
            {
               new RateLimitRule()
                {
                    Endpoint = "*", 
                    Limit = 20,      
                    Period = "1m" 
                },

            };

            services.Configure<IpRateLimitOptions>(
                opt =>
                {
                    opt.GeneralRules = rateLimitRules;
                });

            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

            return services;
        }
    }
}
