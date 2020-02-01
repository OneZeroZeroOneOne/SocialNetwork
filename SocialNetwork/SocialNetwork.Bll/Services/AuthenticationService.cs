using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Claims;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Models;
using SocialNetwork.Bll.Abstractions;
using System.Linq;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;


namespace SocialNetwork.Bll.Services
{

    public class AuthenticationService : IAuthentication
    {
        private readonly PublicContext _context;
        public AuthenticationService(PublicContext publicContext) {

            _context = publicContext;

        }
        public ClaimsIdentity GetIdentity(string username, string password)
        {
            User user = _context.User.FirstOrDefault(x => x.Name == username);

            UserSecurity usersecurity = _context.UserSecurity.FirstOrDefault(x => x.UserId == user.Id && x.Password == password);


            if (usersecurity != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }
    }
}
