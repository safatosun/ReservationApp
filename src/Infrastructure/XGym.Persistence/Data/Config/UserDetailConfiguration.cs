using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XGym.Domain.Entities;

namespace XGym.Persistence.Data.Config
{
    public class UserDetailConfiguration : IEntityTypeConfiguration<UserDetail>
    {
        public void Configure(EntityTypeBuilder<UserDetail> builder)
        {
            builder.Property(r => r.CreatedAt).HasDefaultValueSql("NOW()"); 

            builder.Property(ud => ud.Shoulder)
                .HasPrecision(5, 2);

            builder.Property(ud => ud.Chest)
                   .HasPrecision(5, 2);

            builder.Property(ud => ud.Arm)
                   .HasPrecision(5, 2);

            builder.Property(ud => ud.Waist)
                   .HasPrecision(5, 2);

            builder.Property(ud => ud.Hip)
                   .HasPrecision(5, 2);

            builder.Property(ud => ud.Thigh)
                   .HasPrecision(5, 2);

            builder.HasOne(ud => ud.User).WithOne(u => u.Detail).HasForeignKey<UserDetail>(ud => ud.UserId).OnDelete(DeleteBehavior.Cascade);  
        }
    }
}
