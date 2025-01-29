using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XGym.Domain.Enums;

namespace XGym.Application.ReservationOperations.Queries.GetReservationsWithPagination
{
    public class ReservationsDto
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ReservationStatus Status { get; set; }
        public string? SpecialRequest { get; set; }
        public string UserFullName { get; set; }
    }
}
