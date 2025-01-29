using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGym.Application.Common.Parameter
{
    public class MetaData
    {
        public uint CurrentPage { get; set; }
        public uint TotalPage { get; set; }
        public uint PageSize { get; set; }
        public uint TotalCount { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasPage => CurrentPage < TotalPage;

    }
}
