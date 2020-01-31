using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Claims;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Models;
using SocialNetwork.Bll.Abstractions;

namespace SocialNetwork.Bll.Services
{
    class AuthenticationService : IAuthentication
    {
        private readonly PublicContext _context;
        public AuthenticationService(PublicContext publicContext) {

            _context = publicContext;

        }
        private ClaimsIdentity GetIdentity(string username, string password)
        {
            UserSecurity usersecurity = _context.User.
            if (usersecurity != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
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
