using System.Security.Claims;
using System.Threading.Tasks;

namespace SocialNetwork.Security.Abstractions
{
    public interface IAuthenticationService
    {
        Task<ClaimsIdentity> GetIdentity(string email, string password);

        string GenerateToken(ClaimsIdentity claims);
    }
}
