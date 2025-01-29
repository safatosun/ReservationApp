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
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.Property(r => r.CreatedAt).HasDefaultValueSql("NOW()"); 

            builder.HasOne(r => r.User).WithMany(u=> u.Reservations).HasForeignKey(r=>r.UserId).OnDelete(DeleteBehavior.Cascade); 

        }
    }
}
