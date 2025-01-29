using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XGym.Domain.Common;

namespace XGym.Domain.Entities
{
    public class UserDetail : BaseEntity<int>
    {
        public decimal Shoulder { get; set; }
        public decimal Chest { get; set; }
        public decimal Arm { get; set; }
        public decimal Waist { get; set; }
        public decimal Hip { get; set; }
        public decimal Thigh { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
