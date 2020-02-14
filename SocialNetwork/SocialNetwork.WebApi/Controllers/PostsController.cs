﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels;
using SocialNetwork.Dal.ViewModels.In;
using SocialNetwork.Dal.ViewModels.Out;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialNetwork.Dal.Context;

namespace SocialNetwork.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : SocialNetworkBaseApiController
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly IReactionService _reactionService;
        private readonly ILogger<PostsController> _logger;
        public PostsController(ILogger<PostsController> logger, IMapper mapper, 
            IPostService postService, IReactionService reactionService)
        {
            _logger = logger;
            _mapper = mapper;
            _reactionService = reactionService;
            _postService = postService;
        }

        [HttpGet, Route("{postId}", Name = "postId")]
        public async Task<OutPostViewModel> Get([FromRoute]Guid postId)
        {
            var post = await _postService.GetPost(postId);

            return _mapper.Map<Post, OutPostViewModel>(post);
        }

        [HttpPost]
        [Authorize(Policy = "ConfirmedUser")]
        public async Task<OutPostViewModel> Post([FromBody]PostViewModel post)
        {
            var currentUser = CurrentUser();

            var dataModel = _mapper.Map<PostViewModel, Post>(post);
            var insertedPost = await _postService.CreateNewPost(dataModel, currentUser);

            return _mapper.Map<Post, OutPostViewModel>(insertedPost);
        }

        [HttpPatch]
        [Authorize(Policy = "ConfirmedUser")]
        public async Task<OutPostViewModel> Patch([FromBody]EditPostViewModel post)
        {
            var currentUser = CurrentUser();

            var dataModel = _mapper.Map<EditPostViewModel, Post>(post);
            var insertedPost = await _postService.EditPost(dataModel, currentUser);

            return _mapper.Map<Post, OutPostViewModel>(insertedPost);
        }

        [HttpGet, Route("/Page/", Name = "GetPagePosts")]
        public async Task<PagedResult<Post>> GetPagePosts([FromQuery]int page, [FromQuery]int quantity)
        {
            return await _postService.GetPagePosts(page, quantity);
        }

        [HttpGet, Route("{postId}/Reactions", Name = "GetPostReactions")]
        public async Task<List<OutReactionPostViewModel>> Reaction([FromRoute]Guid postId)
        {
            List<ReactionPost> listReactionPost = await _reactionService.GetReactionPost(postId);
            return listReactionPost.Select(i => _mapper.Map<ReactionPost, OutReactionPostViewModel>(i)).ToList();
        }

        [HttpDelete]
        [Authorize(Policy = "ConfirmedUser")]
        public async Task<IActionResult> DeletePost([FromQuery]Guid postId)
        {
            var currentUser = CurrentUser();
            await _postService.DeletePost(postId, currentUser);

            return Ok();
        }
    }
}