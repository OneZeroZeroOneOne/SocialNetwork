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
    public class PostsController : ControllerBase
    {
        public PublicContext context;
        private readonly ILogger<PostsController> _logger;
        public PostsController(ILogger<PostsController> logger)
        {
            _logger = logger;
            context = new PublicContext();
        }

        [HttpGet]
        public Post Get([FromQuery]Guid postId)
        {
            return context.Post.Where(x => x.Id == postId).FirstOrDefault();
        }

        [HttpPost]
        public OkResult Post([FromBody]Post post)
        {
            context.Post.Add(post);
            context.SaveChanges();
            return Ok();
        }
    }
}