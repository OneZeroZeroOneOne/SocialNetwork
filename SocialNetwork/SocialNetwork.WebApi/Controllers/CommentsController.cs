﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels;
using SocialNetwork.Dal.ViewModels.In;
using SocialNetwork.Dal.ViewModels.Out;
using System.Collections.Generic;
using System.Linq;
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
        public CommentsController(ILogger<PostsController> logger, IMapper mapper,
            ICommentService commentService, IReactionService reactionService)
        {
            _logger = logger;
            _mapper = mapper;
            _reactionService = reactionService;
            _commentService = commentService;

        }

        [HttpGet, Route("{commentId}", Name = "commentId")]
        public async Task<OutCommentViewModel> Get([FromRoute]int commentId)
        {
            var comment = await _commentService.GetComment(commentId);

            return _mapper.Map<Comment, OutCommentViewModel>(comment);
        }

        [HttpPost]
        [Authorize(Policy = "ConfirmedUser")]
        public async Task<OutCommentViewModel> Post([FromBody]CommentViewModel comment)
        {
            var currentUser = CurrentUser();

            var dataModel = _mapper.Map<CommentViewModel, Comment>(comment);
            var insertedComment = await _commentService.AddComment(dataModel, currentUser, comment.AttachmentIdList);

            return _mapper.Map<Comment, OutCommentViewModel>(insertedComment);
        }

        [HttpPatch]
        [Authorize(Policy = "ConfirmedUser")]
        public async Task<OutCommentViewModel> Patch([FromBody]EditCommentViewModel comment)
        {
            var currentUser = CurrentUser();

            var dataModel = _mapper.Map<EditCommentViewModel, Comment>(comment);
            var insertedComment = await _commentService.EditComment(dataModel, currentUser);

            return _mapper.Map<Comment, OutCommentViewModel>(insertedComment);
        }

        [HttpGet, Route("Page/{postId}", Name = "GetPageComments")]
        public async Task<PagedResult<OutCommentViewModel>> GetPageComments([FromRoute]int postId, [FromQuery]int page, [FromQuery]int quantity, [FromQuery]bool sortOrder = false)
        {
            return _mapper.Map<PagedResult<Comment>, PagedResult<OutCommentViewModel>>(await _commentService.GetPageComments(postId, page, quantity, sortOrder));
        }

        [HttpGet, Route("{commentId}/Reactions", Name = "GetCommentReactions")]
        public async Task<List<OutReactionCommentViewModel>> Reaction([FromRoute]int commentId)
        {
            List<ReactionComment> listReactionComment = await _reactionService.GetReactionComment(commentId);
            return listReactionComment.Select(i => _mapper.Map<ReactionComment, OutReactionCommentViewModel>(i)).ToList();
        }

        [HttpDelete]
        [Authorize(Policy = "ConfirmedUser")]
        public async Task<IActionResult> DeleteComment([FromQuery]int commentId)
        {
            var currentUser = CurrentUser();
            await _commentService.DeleteComment(commentId, currentUser);

            return Ok();
        }
    }
}