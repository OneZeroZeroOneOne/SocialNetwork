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
    public class RegisterUserController : ControllerBase
    {
        private PublicContext _context;
        private readonly ILogger<RegisterUserController> _logger;
        public RegisterUserController(ILogger<RegisterUserController> logger)
        {
            _logger = logger;
            _context = new PublicContext();
        }

        [HttpPost]
        public OkResult Post(string name, string password, string mail, string avatarurl)
        {
            
            Guid id = new Guid();
            User user = new User { Name = name, Id = id, Mail = mail, AvatarUrl = avatarurl};
            Console.WriteLine(user.Name);
            UserSecurity usersecurity = new UserSecurity { UserId = id, Password = password, Mail = mail};
            _context.User.Add(user);
            _context.UserSecurity.Add(usersecurity);
            _context.SaveChanges();
            return new OkResult();

            
        }
    }
}
