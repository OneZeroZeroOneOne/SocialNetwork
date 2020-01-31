using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Exceptions;
using SocialNetwork.Security.Abstractions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace SocialNetwork.Security.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly PublicContext _context;

        public AuthenticationService(PublicContext publicContext) 
        {
            _context = publicContext;
        }

        public ClaimsIdentity GetIdentity(string email, string password)
        {
            var user = _context.User.Include(x => x.UserSecurity)
                .FirstOrDefault(x => x.Email == email && x.UserSecurity.Password == password);

            if (user?.UserSecurity == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.UserNotFound, $"User with email {email} not found");

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.UserSecurity.Role),
            };

            return new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }

        public string GenerateToken(ClaimsIdentity claims)
        {
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: claims.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

    }
}