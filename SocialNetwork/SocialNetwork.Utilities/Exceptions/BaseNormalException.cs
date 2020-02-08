using Newtonsoft.Json;
using System;

namespace SocialNetwork.Utilities.Exceptions
{
    public class BaseNormalException : Exception
    {
        public BaseNormalException(int code, string message)
            : base(message)
        {
            Code = code;
            ExMessage = message;
        }

        public int Code { get; set; }
        public string ExMessage { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(new { Code, Message = ExMessage });
        }
    }
}
