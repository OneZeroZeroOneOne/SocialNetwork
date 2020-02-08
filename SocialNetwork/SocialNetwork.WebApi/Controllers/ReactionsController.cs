using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels.In;
using SocialNetwork.Dal.ViewModels.Out;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        [HttpGet, Route("Post", Name = "GetReactionTypePost")]
        public async Task<List<ReactionTypePost>> ReactionTypePost()
        {
            return await _reactionService.GetReactionTypePost();
        }

        [HttpGet, Route("Comment", Name = "GetReactionTypeComment")]
        public async Task<List<ReactionTypeComment>> ReactionTypeComment()
        {
            return await _reactionService.GetReactionTypeComment();
        }

        [HttpPost, Route("Post", Name = "AddReactionPost")]
        [Authorize(Policy = "ConfirmedUser")]
        public async Task<OutReactionPostViewModel> AddReactionPost([FromBody]ReactionPostViewModel reactionPost)
        {
            var currentUser = CurrentUser();
            var dataModel = _mapper.Map<ReactionPostViewModel, ReactionPost>(reactionPost);
            var post = await _reactionService.AddReactionPost(dataModel, currentUser);

            return _mapper.Map<ReactionPost, OutReactionPostViewModel>(post);
        }

        [HttpPost, Route("Comment", Name = "AddReactionComment")]
        [Authorize(Policy = "ConfirmedUser")]
        public async Task<OutReactionCommentViewModel> AddReactionComment([FromBody]ReactionCommentViewModel reactionComment)
        {
            var currentUser = CurrentUser();
            var dataModel = _mapper.Map<ReactionCommentViewModel, ReactionComment>(reactionComment);
            var comment = await _reactionService.AddReactionComment(dataModel, currentUser);

            return _mapper.Map<ReactionComment, OutReactionCommentViewModel>(comment);
        }
    }
}