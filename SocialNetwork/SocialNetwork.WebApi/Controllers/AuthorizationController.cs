using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Models;


namespace SocialNetwork.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizationController : ControllerBase
    {
        public PublicContext context;
        private readonly ILogger<AuthorizationController> _logger;
        public AuthorizationController(ILogger<AuthorizationController> logger)
        {
            _logger = logger;
            context = new PublicContext();
        }

        /*[HttpGet]
        public string GetToken(string name, string password)
        {
            User user = context.User.Where(x => x.Name == name).FirstOrDefault();
            UserSecurity usersecurity = context.UserSecurity.Where(x => x.UserId == user.Id).FirstOrDefault();
            if (usersecurity.Password == password)
            {
                string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                return token;
            }
        }*/
    }
}
