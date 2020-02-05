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
using SocialNetwork.Dal.Extensions;

namespace SocialNetwork.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReactionController : SocialNetworkBaseApiController
    {
        private readonly IReactionService _reactionService;
        private readonly IMapper _mapper;

        private readonly ILogger<ReactionController> _logger;
        public ReactionController(ILogger<ReactionController> logger, IMapper mapper, IReactionService reactionService)
        {
            _logger = logger;
            _mapper = mapper;

            _reactionService = reactionService;
        }

        [HttpPost, Route("/Post/{postId}", Name = "GetPagePosts")]
        [Authorize(Roles = "Member")]
        public async Task<List<OutReactionCommentViewModel>> AddReactionPost([FromRoute]ReactionPostViewModel post)
        {
            var currentUser = await CurrentUser();
            var dataModel = _mapper.Map<PostViewModel, Post>(post);
            var post = await _reactionService.AddPostReaction(postId);

            List<OutReactionCommentViewModel> listReactions = new List<OutReactionCommentViewModel>();


            return _mapper.Map<Post, OutReactionCommentViewModel>(post);
        }

        [HttpPost, Route("/Comment/{commentId}", Name = "GetPagePosts")]
        [Authorize(Roles = "Member")]
        public async Task<List<OutReactionCommentViewModel>> AddReactionComment([FromRoute]ReactionPostViewModel comment)
        {
            var currentUser = await CurrentUser();
            var post = await _reactionService.GetPostReaction(reactionPost);

            List<OutReactionCommentViewModel> listReactions = new List<OutReactionCommentViewModel>();


            return _mapper.Map<Post, OutReactionCommentViewModel>(post);
        }
    }
