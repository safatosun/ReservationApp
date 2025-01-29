using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGym.Application.Common.Parameter
{
    public class RequestParameters
    {
        const uint maxPageSize = 20;
        public uint PageNumber { get; set; }
        private uint _pageSize;

        public uint PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > maxPageSize ? maxPageSize : value; }
        }

        public RequestParameters()
        {
            PageNumber = 1;
            PageSize = 20;
        }


    }
}
