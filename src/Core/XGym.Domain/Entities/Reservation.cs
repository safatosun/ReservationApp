using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XGym.Domain.Common;
using XGym.Domain.Enums;

namespace XGym.Domain.Entities
{
    public class Reservation : BaseEntity<int>
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ReservationStatus Status { get; set; }
        public string? SpecialRequest { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
