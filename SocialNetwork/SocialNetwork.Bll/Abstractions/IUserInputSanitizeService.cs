using System.Threading.Tasks;

namespace SocialNetwork.Bll.Abstractions
{
    public interface IUserInputSanitizeService
    {
        Task<string> SanitizeHtml(string rawHtml);
    }
}