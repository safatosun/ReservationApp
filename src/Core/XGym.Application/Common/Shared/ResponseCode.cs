using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGym.Application.Common.Shared
{
    public enum ResponseCode
    {
        Success = 200,
        Created = 201,
        NoContent = 204,
        BadRequest = 400,
        UnAuthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        Conflict = 409,
        TooManyRequests = 429,
        Fail = 500,
    }
}
