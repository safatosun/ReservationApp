using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGym.Application.UserDetailOperations.Queries.GetUsersDetailsWithPagination
{
    public class UsersDetailsDto
    {
        public int Id { get; set; }
        public decimal Shoulder { get; set; }
        public decimal Chest { get; set; }
        public decimal Arm { get; set; }
        public decimal Waist { get; set; }
        public decimal Hip { get; set; }
        public decimal Thigh { get; set; }
        public string FullName { get; set; }
    }
}
