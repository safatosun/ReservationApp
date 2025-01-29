using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGym.Application.Common.Shared
{
    public class Response
    {
        public ResponseCode ResponseCode { get; }
        public string Message { get; }

        public Response(ResponseCode responseCode)
        {
            ResponseCode = responseCode;
            Message = responseCode.ToString();
        }

        public Response(ResponseCode responseCode, string message)
        {
            ResponseCode = responseCode;
            Message = message;
        }

    }

    public class Response<T>
    {
        public ResponseCode ResponseCode { get; }
        public string Message { get; }
        public T? Data { get; }

        public Response(ResponseCode responseCode)
        {
            ResponseCode = responseCode;
            Message = responseCode.ToString();
        }

        public Response(ResponseCode responseCode, string message)
        {
            ResponseCode = responseCode;
            Message = message;
        }

        public Response(ResponseCode responseCode, T data)
        {
            ResponseCode = responseCode;
            Data = data;
            Message = responseCode.ToString();
        }

        public Response(ResponseCode responseCode, T data, string message)
        {
            ResponseCode = responseCode;
            Data = data;
            Message = message;
        }
    }

}
