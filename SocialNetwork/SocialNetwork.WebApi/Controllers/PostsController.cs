using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels;
using System;
using System.Threading.Tasks;

namespace SocialNetwork.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : SocialNetworkBaseApiController
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

        [HttpGet, Route("{postId}", Name = "postId")]
        public IActionResult Get([FromRoute]Guid postId)
        {
            var post = _postService.GetPost(postId);
            if (post == null)
                return NotFound();

            var returnModel = _mapper.Map<Post, OutPostViewModel>(post);
            return Ok(returnModel);
        }

        [HttpPost]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Post([FromBody]PostViewModel post)
        {
            var currentUser = await CurrentUser();

            var dataModel = _mapper.Map<PostViewModel, Post>(post);
            var insertedPost = _postService.CreateNewPost(dataModel, currentUser);

            var returnModel = _mapper.Map<Post, OutPostViewModel>(insertedPost);
            return Ok(returnModel);
        }
    }
}