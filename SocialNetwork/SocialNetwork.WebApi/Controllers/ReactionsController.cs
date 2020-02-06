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
    public class ReactionsController : SocialNetworkBaseApiController
    {
        private readonly IReactionService _reactionService;
        private readonly IMapper _mapper;
        private readonly ILogger<ReactionsController> _logger;
        public ReactionsController(ILogger<ReactionsController> logger, IMapper mapper, IReactionService reactionService)
        {
            _logger = logger;
            _mapper = mapper;
            _reactionService = reactionService;
        }

        [HttpPost, Route("Post", Name = "AddReactionPost")]
        [Authorize(Roles = "Member")]
        public async Task<OutReactionPostViewModel> AddReactionPost([FromBody]ReactionPostViewModel reactionPost)
        {
            var currentUser = await CurrentUser();
            var dataModel = _mapper.Map<ReactionPostViewModel, ReactionPost>(reactionPost);
            var post = await _reactionService.AddReactionPost(dataModel, currentUser);

            return _mapper.Map<ReactionPost, OutReactionPostViewModel>(post);
        }

        [HttpPost, Route("Comment", Name = "AddReactionComment")]
        [Authorize(Roles = "Member")]
        public async Task<OutReactionCommentViewModel> AddReactionComment([FromBody]ReactionCommentViewModel reactionComment)
        {
            var currentUser = await CurrentUser();
            var dataModel = _mapper.Map<ReactionCommentViewModel, ReactionComment>(reactionComment);
            var comment = await _reactionService.AddReactionComment(dataModel, currentUser);

            return _mapper.Map<ReactionComment, OutReactionCommentViewModel>(comment);
        }
    }
}