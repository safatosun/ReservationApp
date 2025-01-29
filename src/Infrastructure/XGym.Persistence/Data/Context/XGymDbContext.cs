using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XGym.Application.Common.Interfaces;
using XGym.Application.Common.Utility;
using XGym.Domain.Entities;

namespace XGym.Persistence.Data.Context
{
    public class XGymDbContext : IdentityDbContext<User>, IXGymDbContext
    {
        private readonly HttpContext _httpContext;

        public XGymDbContext(DbContextOptions options,IHttpContextAccessor contextAccessor) : base(options)
        {
            if (contextAccessor.HttpContext != null)
            {
                _httpContext = contextAccessor.HttpContext;
            }
        }


        public DbSet<User> User { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

         public async Task<int> SaveChangesAsync()
        {
            var apiUserId = _httpContext.User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;

            var entries = ChangeTracker.Entries().Where(e => e.Entity is User || e.Entity is UserDetail || e.Entity is Reservation)
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {

                if (entry.Entity is User user)
                {

                    if (entry.State == EntityState.Modified)
                    {
                        user.ModifiedAt = DateTime.Now;
                        user.ModifiedUserId = apiUserId;
                    }
                }

                if (entry.Entity is UserDetail userDetail)
                {
                    if (entry.State == EntityState.Added)
                    {
                        userDetail.CreatedUserId = apiUserId;
                    }
                    if (entry.State == EntityState.Modified)
                    {
                        userDetail.ModifiedAt = DateTime.Now;
                        userDetail.ModifiedUserId = apiUserId; 
                    }
                }

                if (entry.Entity is Reservation reservation)
                {
                    if (entry.State == EntityState.Added)
                    {
                        reservation.CreatedUserId = apiUserId; 
                    }
                    if (entry.State == EntityState.Modified)
                    {
                        reservation.ModifiedAt = DateTime.Now;
                        reservation.ModifiedUserId = apiUserId; 
                    }
                }
            }

            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
