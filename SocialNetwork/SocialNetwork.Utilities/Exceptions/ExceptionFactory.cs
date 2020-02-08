namespace SocialNetwork.Utilities.Exceptions
{
    public class ExceptionFactory
    {
        public static BaseErrorException SoftException(ExceptionEnum code, string message, params object[] args)
        {
            message = string.Format(message, args);
            return new BaseErrorException((int)code, message);
        }

        public static BaseErrorException SoftException(ExceptionEnum code, string message)
        {
            return new BaseErrorException((int)code, message);
        }

        public static BaseNormalException UserFriendlyException(ExceptionEnum code, string message)
        {
            return new BaseNormalException((int)code, message);
        }
    }
}
