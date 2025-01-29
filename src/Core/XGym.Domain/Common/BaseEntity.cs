using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGym.Domain.Common
{
    public class BaseEntity<T>
    {
        public T Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedUserId { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedUserId { get; set; }
    }
}
