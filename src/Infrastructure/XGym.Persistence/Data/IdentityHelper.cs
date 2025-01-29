using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XGym.Domain.Entities;
using XGym.Persistence.Data.Context;

namespace XGym.Persistence.Data
{
    public class IdentityHelper
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var dbContext = serviceProvider.GetRequiredService<XGymDbContext>())
            {
                dbContext.Database.Migrate();
                var userManager = serviceProvider.GetService<UserManager<User>>();
                var userToCreate = new User
                {
                    Name = "admin",
                    Surname = "admin",
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@xgym.com",
                    NormalizedEmail = "ADMIN@XGYM.COM",
                    EmailConfirmed = true,
                    PhoneNumber = "1112223344",
                    PhoneNumberConfirmed = true,
                    PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "Admin@123@?"),
                };
                
                await userManager.CreateAsync(userToCreate);                
                
                await userManager.AddToRoleAsync(userToCreate, "Admin");
            }
        }
    }
}
