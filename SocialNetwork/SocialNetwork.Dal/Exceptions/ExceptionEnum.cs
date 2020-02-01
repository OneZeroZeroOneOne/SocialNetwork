namespace SocialNetwork.Dal.Exceptions
{
    public enum ExceptionEnum
    {
        UserNotFound = 1,
        UserAlreadyExist = 2,
        PostNotFound = 3,
        CommentNotFound = 4,
        EmailFormatInvalid = 5,
        PasswordNotValid = 6,
        EmailNotConfirmed = 7,
        CantSendEmail = 8,
        CantFindConfirmationToken = 9,
        PasswordIncorrect = 10,
    }
}
