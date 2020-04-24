using System.Threading.Tasks;

namespace SocialNetwork.Bll.Abstractions
{
    public interface IUserInputService
    {
        Task<string> SanitizeHtml(string rawHtml);
    }
}