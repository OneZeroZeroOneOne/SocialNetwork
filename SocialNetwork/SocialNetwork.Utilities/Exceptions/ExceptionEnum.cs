namespace SocialNetwork.Utilities.Exceptions
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
        CantLoadSecrets = 11,
        InappropriatParameters = 12,
        ReactionDoesNotExist = 13,
        ReactionAlreadyExist = 14,
        BoardDoesntExist = 15,
        ProvideFile = 16,
    }
}
