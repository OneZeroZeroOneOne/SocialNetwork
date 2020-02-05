﻿using Newtonsoft.Json;
using System;

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
            return JsonConvert.SerializeObject(new {Code, Message = ExMessage});
        }
    }
}