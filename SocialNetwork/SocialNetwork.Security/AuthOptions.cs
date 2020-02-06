using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SocialNetwork.Security
{
    public class AuthOptions
    {
        public const string Issuer = "SocialNetwork.AuthorizationServer"; // auth service
        public const string Audience = "SocialNetwork.WebClient"; // client
        public const int Lifetime = 12 * 60; // 12 hours

        private const string Key = "my_very_long_secret_key_i_know_its_issue";   // security key
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
        }
    }
}
