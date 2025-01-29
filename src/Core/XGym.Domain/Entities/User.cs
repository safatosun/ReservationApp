using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XGym.Domain.Common;

namespace XGym.Domain.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpireTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedUserId { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedUserId { get; set; }

        public UserDetail Detail { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}
