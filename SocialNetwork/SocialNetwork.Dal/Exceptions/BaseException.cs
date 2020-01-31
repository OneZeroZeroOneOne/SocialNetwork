using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Dal.Exceptions
{
    public class BaseException : Exception
    {
        public BaseException(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public int Code { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
