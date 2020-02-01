using System;
using System.Collections.Generic;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Exceptions;
using SocialNetwork.Dal.Models;
using SocialNetwork.Security.Abstractions;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using User = SocialNetwork.Dal.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SocialNetwork.Security.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly PublicContext _publicContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;

        private readonly Regex _emailRegex;

        private readonly Regex _hasNumber;
        private readonly Regex _hasUpperChar;
        private readonly Regex _hasMinimum8Chars;

        public RegistrationService(PublicContext publicContext, IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _publicContext = publicContext;
            _httpContextAccessor = httpContextAccessor;
            _config = config;

            _hasNumber = new Regex(@"[0-9]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            _hasUpperChar = new Regex(@"[A-Z]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            _hasMinimum8Chars = new Regex(@".{8,}", RegexOptions.Compiled | RegexOptions.IgnoreCase);


            _emailRegex = new Regex(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*\"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public async Task<bool> ConfirmEmail(Guid confirmId)
        {
            var userConfirmationToken = await 
                _publicContext.UserConfirmationToken.Where(x => x.ConfirmId == confirmId)
                    .Include(x => x.UserSecurity)
                    .FirstOrDefaultAsync();

            if (userConfirmationToken == null)
                return false;/*throw ExceptionFactory.SoftException(ExceptionEnum.CantFindConfirmationToken,
                $"Can't find this confirmation token!");*/

            if (!userConfirmationToken.IsActive) //maybe need additional logic after those step idk
                return false;

            userConfirmationToken.UserSecurity.Role = "Member";
            userConfirmationToken.UserSecurity.IsConfirmed = true;
            userConfirmationToken.IsActive = false;

            _publicContext.UserConfirmationToken.Update(userConfirmationToken);
            await _publicContext.SaveChangesAsync();

            return true;
        }

        public async Task<User> Register(string email, string password)
        {
            var isPasswordValid = _hasNumber.IsMatch(password) && _hasUpperChar.IsMatch(password) && _hasMinimum8Chars.IsMatch(password);

            if(!isPasswordValid)
                throw ExceptionFactory.SoftException(ExceptionEnum.PasswordNotValid, $"Password need to follow:\n\tMinimum 8 char\n\tHave at least one number\n\tHave upper char");

            if (!_emailRegex.IsMatch(email))
                throw ExceptionFactory.SoftException(ExceptionEnum.EmailFormatInvalid, $"Email {email} have wrong format");

            if (_publicContext.User.FirstOrDefault(x => x.Email == email) != null)
                throw ExceptionFactory.SoftException(ExceptionEnum.UserAlreadyExist,
                    $"User with email {email} already exist");

            var confirmToken = new UserConfirmationToken()
            {
                ConfirmEmail = email,
                ConfirmId = Guid.NewGuid(),
                IsActive = true,
            };

            if (!await SendConfirmEmail(email, confirmToken.ConfirmId.ToString()))
                throw ExceptionFactory.SoftException(ExceptionEnum.CantSendEmail,
                    $"Can't send confirmation email. Try later");

            User userModel = new User()
            {
                Email = email,
                UserSecurity = new UserSecurity()
                {
                    Password = password,
                    Role = "PreMember", //PreMember because email not confirmed!
                    IsConfirmed = false,
                    UserConfirmationTokens = new List<UserConfirmationToken>()
                    {
                        confirmToken,
                    },
                },
            };

            await _publicContext.User.AddAsync(userModel);
            await _publicContext.SaveChangesAsync();

            return userModel;
        }

        public async Task<bool> SendConfirmEmail(string email, string confirmToken)
        {

            var apiSecret = _config.GetSection("MailJet").GetSection("ApiSecret").Value;
            var apiKey = _config.GetSection("MailJet").GetSection("ApiKey").Value;

            if (apiKey == null || apiSecret == null)
                return false;

            MailjetClient client = new MailjetClient(apiKey, apiSecret)
            {
                Version = ApiVersion.V3_1,
            };

            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
            .Property(Send.Messages, new JArray 
            {
                new JObject 
                {
                    {
                        "From",
                        new JObject 
                        {
                            {"Email", "zebestforevka@gmail.com"},
                            {"Name", "SocialNetwork"}
                        }
                    }, {
                        "To",
                        new JArray 
                        {
                            new JObject 
                            {
                                {
                                    "Email", email
                                }
                            }
                        }
                    }, {
                        "Subject",
                        "Confirm your email"
                    }, {
                        "TextPart",
                        "Confirm your email"
                    }, {
                        "HTMLPart",
                        $"<h3>Hello, thank for registering in SocialNetwork!</h3></br><a href='{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/Registration?confirmationToken={confirmToken}'>Confirm</a> your email for using our service!"
                    }
                }
            });

            MailjetResponse response = await client.PostAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}
