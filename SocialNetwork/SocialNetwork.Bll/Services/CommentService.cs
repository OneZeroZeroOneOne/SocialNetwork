using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Extensions;
using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels;
using SocialNetwork.RequestLifetimeBll.Abstractions;
using SocialNetwork.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialNetwork.Dal.Dapper;

namespace SocialNetwork.Bll.Services
{

    public class CommentService : ICommentService
    {
        private readonly PublicContext _context;
        private readonly IUserInputService _userInputService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRequestLifetimeService _requestLifetimeService;
        private readonly IHttpContextAccessor _httpContext;

        public CommentService(PublicContext publicContext, IUserInputService userInputService, IServiceProvider serviceProvider, IRequestLifetimeService requestLifetimeService, IHttpContextAccessor httpContext)
        {
            _context = publicContext;
            _userInputService = userInputService;
            _serviceProvider = serviceProvider;
            _requestLifetimeService = requestLifetimeService;
            _httpContext = httpContext;
        }


        private async Task AttachFileToComment(int commentId, int attachmentId)
        {
            if (await _context.Attachment.AnyAsync(x => x.Id == attachmentId))
            {
                var attachmentComment = new AttachmentComment()
                {
                    AttachmentId = attachmentId,
                    CommentId = commentId
                };

                await _context.AttachmentComment.AddAsync(attachmentComment);
            }
        }

        public async Task<Comment> AddComment(Comment commentModel, Guid authorUser, List<int> attachmentIdList)
        {
            var post = await _context.Post
                .Include(x => x.Board)
                .FirstOrDefaultAsync(x => x.Id == commentModel.PostId);

            if (post == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.PostNotFound, $"Post {commentModel.PostId} doesn't exist");

            commentModel.UserId = authorUser;

            _requestLifetimeService.SetPost(post);
            _requestLifetimeService.SetBoard(post.Board);

            _httpContext.HttpContext.Items.Add("RequestLifetime", _requestLifetimeService);
            commentModel.PostId = post.Id;
            commentModel.Text = await _userInputService.Markdown(commentModel.Text);

            /*post.Comments.Add(commentModel);

            var insertedPost = _context.Update(post);

            await _context.SaveChangesAsync();*/

            var insertedComment = _context.AddComment(commentModel);

            if (attachmentIdList != null)
                foreach (var attachmentId in attachmentIdList)
                {
                    await AttachFileToComment(insertedComment.Id, attachmentId);
                }

            await _context.SaveChangesAsync();

            return await GetComment(insertedComment.Id);
        }

        public async Task<Comment> EditComment(Comment commentModel, Guid editorUser)
        {
            var comment = await _context.Comment.FirstOrDefaultAsync(x => x.Id == commentModel.Id);

            if (comment == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.CommentNotFound, $"Comment {commentModel.Id} doesn't exist");

            comment.Text = await _userInputService.Markdown(commentModel.Text);

            _context.Comment.Update(comment);
            await _context.SaveChangesAsync();

            return comment;
        }

        public async Task<Comment> GetComment(int commentId)
        {
            var comment = await _context.Comment
                .Include(x => x.AttachmentComment)
                    .ThenInclude(x => x.Attachment)
                .FirstOrDefaultAsync(x => x.Id == commentId && x.IsArchived == false);

            if (comment == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.CommentNotFound, $"Comment {commentId} doesn't exist");

            return comment;
        }
        public async Task<PagedResult<Comment>> GetPageComments(int postId, int page, int quantity, bool sortOrder = false)
        {
            if (page <= 0 || quantity <= 0)
                throw ExceptionFactory.SoftException(ExceptionEnum.InappropriatParameters,
                    "Page and quantity must be real number");

            return await _context.Comment.Where(x => x.PostId == postId && x.IsArchived == false)
                .Include(x => x.AttachmentComment)
                    .ThenInclude(x => x.Attachment)
                .AsQueryable().GetPaged(page, quantity, sortOrder);
        }
        public async Task DeleteComment(int commentId, Guid currentUserId)
        {
            var comment = await _context.Comment.FirstOrDefaultAsync(x => x.Id == commentId && x.UserId == currentUserId);
            if (comment == null)
            {
                throw ExceptionFactory.SoftException(ExceptionEnum.CommentNotFound,
                    $"Comment with {commentId} comment id not exist");
            }
            comment.IsArchived = true;
            _context.Comment.Update(comment);
            await _context.SaveChangesAsync();
        }
    }
}