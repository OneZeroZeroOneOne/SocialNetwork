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
using System.Threading.Tasks;

namespace SocialNetwork.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentsController : SocialNetworkBaseApiController
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;
        private readonly IReactionService _reactionService;
        private readonly ILogger<PostsController> _logger;
        public CommentsController(ILogger<PostsController> logger, IMapper mapper, ICommentService commentService,
                                                                                    IReactionService reactionService)
        {
            _logger = logger;
            _mapper = mapper;
            _reactionService = reactionService;
            _commentService = commentService;
        }

        [HttpGet, Route("{commentId}", Name = "commentId")]
        public async Task<OutCommentViewModel> Get([FromRoute]Guid commentId)
        {
            var comment = await _commentService.GetComment(commentId);

            return _mapper.Map<Comment, OutCommentViewModel>(comment);
        }

        [HttpPost]
        [Authorize(Roles = "Member")]
        public async Task<OutCommentViewModel> Post([FromBody]CommentViewModel comment)
        {
            var currentUser = await CurrentUser();

            var dataModel = _mapper.Map<CommentViewModel, Comment>(comment);
            var insertedComment = await _commentService.AddComment(dataModel, currentUser);

            return _mapper.Map<Comment, OutCommentViewModel>(insertedComment);
        }

        [HttpPatch]
        [Authorize(Roles = "Member")]
        public async Task<OutCommentViewModel> Patch([FromBody]EditCommentViewModel comment)
        {
            var currentUser = await CurrentUser();

            var dataModel = _mapper.Map<EditCommentViewModel, Comment>(comment);
            var insertedComment = await _commentService.EditComment(dataModel, currentUser);

            return _mapper.Map<Comment, OutCommentViewModel>(insertedComment);
        }

        [HttpGet, Route("/CommentPage/{postId}", Name = "GetPageСomments")]
        public async Task<PagedResult<Comment>> GetPageComments([FromRoute]Guid postId, [FromQuery]int page, [FromQuery]int quantity)
        {
            return await _commentService.GetPageComments(postId, page, quantity);
        }

        [HttpGet, Route("{commentId}/Reactions", Name = "GetCommentReactions")]
        public async Task<List<OutReactionCommentViewModel>> Reaction([FromRoute]Guid commentId)
        {
            List<ReactionComment> listReactionComment = await _reactionService.GetReactionComment(commentId);
            List<OutReactionCommentViewModel> listOutReactionComment = new List<OutReactionCommentViewModel>();
            foreach (ReactionComment i in listReactionComment)
            {
                listOutReactionComment.Add(_mapper.Map<ReactionComment, OutReactionCommentViewModel>(i));
            }
            return listOutReactionComment;
        }
    }
}