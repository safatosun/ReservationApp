using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGym.Application.Common.Parameter
{
    public class ReservationRequestParameters : RequestParameters
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
