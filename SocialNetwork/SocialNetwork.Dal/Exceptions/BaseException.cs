using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Dal.Exceptions
{
    public class BaseException : Exception
    {
        public BaseException(int code, string message) 
            : base(message)
        {
            Code = code;
            ExMessage = message;
        }

        public int Code { get; set; }
        public string ExMessage { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
