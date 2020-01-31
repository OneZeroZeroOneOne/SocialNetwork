using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace SocialNetwork.Security
{
    public class AuthOptions
    {
        public const string ISSUER = "SocialNetwork.Security"; // издатель токена
        public const string AUDIENCE = "SocialNetwork.WebClient"; // потребитель токена
        const string KEY = "volodia_loh_huita ebanaya test";   // ключ для шифрации
        public const int LIFETIME = 5; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
        }
    }
}
