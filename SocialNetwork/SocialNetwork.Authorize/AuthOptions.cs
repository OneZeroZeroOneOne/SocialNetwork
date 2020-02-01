using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SocialNetwork.Security
{
    public class AuthOptions
    {
        public const string ISSUER = "SocialNetwork.Security"; // auth service
        public const string AUDIENCE = "SocialNetwork.WebClient"; // client
        const string KEY = "my_very_long_secret_key_i_know_its_issue";   // security key
        public const int LIFETIME = 1 * 60; // one hour
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
        }
    }
}
