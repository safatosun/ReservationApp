using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGym.Persistence.Data.Config
{
    public class RoleConfig : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "cd1c7297-10ab-4a41-a3ce-9f60ae7015b7",
                    Name = "User",
                    NormalizedName = "USER",
                },
                 new IdentityRole
                 {
                     Id = "764b5687-d848-4bd5-9536-f3eb914ade0a",
                     Name = "Admin",
                     NormalizedName = "ADMIN",
                 }
             );
        }
    }
}
