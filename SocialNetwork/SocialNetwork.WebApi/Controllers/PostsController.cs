using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using SocialNetwork.Dal.ViewModels.In;
using SocialNetwork.Dal.ViewModels.Out;
using System.Collections.Generic;

namespace SocialNetwork.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : SocialNetworkBaseApiController
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        private readonly ILogger<PostsController> _logger;
        public PostsController(ILogger<PostsController> logger, IMapper mapper, IPostService postService)
        {
            _logger = logger;
            _mapper = mapper;

            _postService = postService;
        }

        [HttpGet, Route("{postId}", Name = "postId")]
        public async Task<OutPostViewModel> Get([FromRoute]Guid postId)
        {
            var post = await _postService.GetPost(postId);

            return _mapper.Map<Post, OutPostViewModel>(post);
        }

        [HttpPost]
        [Authorize(Roles = "Member")]
        public async Task<OutPostViewModel> Post([FromBody]PostViewModel post)
        {

            var currentUser = await CurrentUser();

            var dataModel = _mapper.Map<PostViewModel, Post>(post);
            var insertedPost = await _postService.CreateNewPost(dataModel, currentUser);

            return _mapper.Map<Post, OutPostViewModel>(insertedPost);
        }

        [HttpPatch]
        [Authorize(Roles = "Member")]
        public async Task<OutPostViewModel> Patch([FromBody]EditPostViewModel post)
        {
            var currentUser = await CurrentUser();

            var dataModel = _mapper.Map<EditPostViewModel, Post>(post);
            var insertedPost = await _postService.EditPost(dataModel, currentUser);

            return _mapper.Map<Post, OutPostViewModel>(insertedPost);
        }
        
        
        
    }
}