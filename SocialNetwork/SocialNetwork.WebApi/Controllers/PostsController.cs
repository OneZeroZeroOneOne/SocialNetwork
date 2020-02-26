using AutoMapper;
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
using System.Numerics;
using System.Threading.Tasks;

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

        [HttpGet, Route("{boardId}/{postId}", Name = "postId")]
        public async Task<OutPostViewModel> Get([FromRoute]Guid boardId, [FromRoute]int postId)
        {
            var post = await _postService.GetPost(boardId, postId);

            return _mapper.Map<Post, OutPostViewModel>(post);
        }

        [HttpPost]
        [Authorize(Policy = "ConfirmedUser")]
        public async Task<OutPostViewModel> Post([FromBody]PostViewModel post)
        {
            var currentUser = CurrentUser();
            var dataModel = _mapper.Map<PostViewModel, Post>(post);
            var insertedPost = await _postService.CreateNewPost(dataModel, currentUser, post.AttachmentIdList);
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

        [HttpGet, Route("{boardId}/Page", Name = "GetPagePosts")]
        public async Task<PagedResult<OutPostViewModel>> GetPagePosts([FromRoute]Guid boardId, [FromQuery]int page, [FromQuery]int quantity)
        {
            return _mapper.Map<PagedResult<Post>, PagedResult<OutPostViewModel>>(await _postService.GetPagePosts(boardId, page, quantity));
        }

        [HttpGet, Route("{boardId}/{postId}/Reactions", Name = "GetPostReactions")]
        public async Task<List<OutReactionPostViewModel>> Reaction([FromRoute]int postId)
        {
            List<ReactionPost> listReactionPost = await _reactionService.GetReactionPost(postId);
            return listReactionPost.Select(i => _mapper.Map<ReactionPost, OutReactionPostViewModel>(i)).ToList();
        }

        [HttpDelete, Route("{boardId}/{postId}")]
        [Authorize(Policy = "ConfirmedUser")]
        public async Task<IActionResult> DeletePost([FromRoute]Guid boardId, [FromRoute]int postId)
        {
            var currentUser = CurrentUser();
            await _postService.DeletePost(boardId, postId, currentUser);

            return Ok();
        }
    }
}