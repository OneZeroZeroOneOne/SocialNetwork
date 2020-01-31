using System.Security.Claims;

namespace SocialNetwork.Security.Abstractions
{
    public interface IAuthenticationService
    {
        ClaimsIdentity GetIdentity(string email, string password);

        string GenerateToken(ClaimsIdentity claims);
    }
}
