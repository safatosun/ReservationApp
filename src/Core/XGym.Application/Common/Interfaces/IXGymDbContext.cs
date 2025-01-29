using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using XGym.Domain.Entities;

namespace XGym.Application.Common.Interfaces
{
    public interface IXGymDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<UserDetail> UserDetails { get; set; }
        DbSet<Reservation> Reservations { get; set; }
        Task<int> SaveChangesAsync();
    }
}
