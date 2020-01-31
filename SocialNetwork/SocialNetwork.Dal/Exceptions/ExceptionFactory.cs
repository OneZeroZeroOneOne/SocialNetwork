using System;

namespace SocialNetwork.Dal.Exceptions
{
    public class ExceptionFactory
    {
        public static Exception SoftException(ExceptionEnum code, string message, params object[] args)
        {
            message = string.Format(message, args);
            return new BaseException((int)code, message);
        }

        public static Exception SoftException(ExceptionEnum code, string message)
        {
            return new BaseException((int)code, message);
        }
    }
}
