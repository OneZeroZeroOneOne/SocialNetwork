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
    public class WritePostController : ControllerBase
    {
        public PublicContext context;
        private readonly ILogger<WritePostController> _logger;
        public WritePostController(ILogger<WritePostController> logger)
        {
            _logger = logger;
            context = new PublicContext();
        }

        [HttpGet]
        public User Get(string token)
        {
            Guid id = new Guid();
            User user = new User { Name = name, Id = id };
            Console.WriteLine(user.Name);
            context.User.Add(user);
            context.SaveChanges();
            return user;
        }
    }
}