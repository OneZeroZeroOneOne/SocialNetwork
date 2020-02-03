using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels;
using System;
using System.Threading.Tasks;
using SocialNetwork.Dal.ViewModels.In;
using SocialNetwork.Dal.ViewModels.Out;
using System.Collections.Generic;
using System.Linq;

namespace SocialNetwork.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentsController : SocialNetworkBaseApiController
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        private readonly ILogger<PostsController> _logger;
        public CommentsController(ILogger<PostsController> logger, IMapper mapper, ICommentService commentService)
        {
            _logger = logger;
            _mapper = mapper;

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

        [HttpGet, Route("/Page/{postId}", Name = "GetPageСomments")]
        public async Task<List<OutCommentViewModel>> GetPageComments([FromRoute]Guid postId, [FromQuery]int page, [FromQuery]int quantity)
        {
            List<Comment> listComment = await _commentService.GetPageComments(postId, page, quantity);
            
            List<OutCommentViewModel> listOutComment = new List<OutCommentViewModel>();

            foreach (Comment i in listComment)
            {
                listOutComment.Add(_mapper.Map<Comment, OutCommentViewModel>(i));
            }
            return listOutComment;
        }
    }
}