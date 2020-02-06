using System.Threading.Tasks;

namespace SocialNetwork.Utilities.Abstractions
{
    public interface IMailClient
    {
        Task<bool> SendConfirmationMail(string email, string confirmUrl);
    }
}
