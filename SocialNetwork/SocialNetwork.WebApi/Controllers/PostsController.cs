using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialNetwork.Bll;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Bll.Services;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels;

namespace SocialNetwork.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private IPostService _postService;
        private readonly IMapper _mapper;

        private readonly ILogger<PostsController> _logger;
        public PostsController(ILogger<PostsController> logger, IMapper mapper, IPostService postService)
        {
            _logger = logger;
            _mapper = mapper;

            //context = new PublicContext();
            _postService = postService;
        }

        [HttpGet]
        public IActionResult Get([FromQuery]Guid postId)
        {
            var post = _postService.GetPost(postId);
            if (post == null)
                return NotFound();

            var returnModel = _mapper.Map<Post, OutPostViewModel>(post);
            return Ok(returnModel);
        }

        [HttpPost]
        public IActionResult Post([FromBody]PostViewModel post)
        {
            var dataModel = _mapper.Map<PostViewModel, Post>(post);
            var insertedPost = _postService.CreateNewPost(dataModel);

            var returnModel = _mapper.Map<Post, OutPostViewModel>(insertedPost);
            return Ok(returnModel);
        }
    }
}