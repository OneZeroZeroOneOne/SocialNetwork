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
using System.Threading.Tasks;

namespace SocialNetwork.Security.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly PublicContext _context;

        public AuthenticationService(PublicContext publicContext) 
        {
            _context = publicContext;
        }

        public async Task<ClaimsIdentity> GetIdentity(string email, string password)
        {
            var user = await _context.User.Include(x => x.UserSecurity)
                .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email == email);

            if (user?.UserSecurity == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.UserNotFound, $"User with email {email} not found");

            if (user.UserSecurity.Password != password)
                throw ExceptionFactory.SoftException(ExceptionEnum.PasswordIncorrect, "Password is incorrect");

            /*Ignore user not confirmed email because they have role preMember
             if (user.UserSecurity.IsConfirmed == false)
                throw ExceptionFactory.SoftException(ExceptionEnum.EmailNotConfirmed, $"Please confirm your email. Check spam folder!");*/


            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.UserSecurity.Role.RoleName),
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

            return "Bearer " + new JwtSecurityTokenHandler().WriteToken(jwt);
        }

    }
}