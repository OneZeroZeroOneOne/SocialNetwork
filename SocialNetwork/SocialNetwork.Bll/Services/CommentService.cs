using Microsoft.EntityFrameworkCore;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Extensions;
using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels;
using SocialNetwork.Dal.ViewModels.In;
using SocialNetwork.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Bll.Services
{

    public class CommentService : ICommentService
    {
        private readonly PublicContext _context;
        public CommentService(PublicContext publicContext)
        {
            _context = publicContext;
        }

        public async Task<Comment> AddComment(CommentViewModel commentModel, Guid authorUser)
        {
            var insertedComment = await _context.Comment.AddAsync(new Comment()
            {
                UserId = authorUser,
                Text = commentModel.Text,
            });

            await _context.SaveChangesAsync();

            var comments = _context.Comment.Where(x => commentModel.ToComment.Contains(x.Id));
            var posts = _context.Post.Where(x => commentModel.ToPost.Contains(x.Id));

            var linkedComments = new List<CommentComment>();
            var linkedPost = new List<CommentPost>();

            foreach (var comment in comments)
            {
                linkedComments.Add(new CommentComment()
                {
                    CommentId = insertedComment.Entity.Id,
                    ReplyToCommentId = comment.Id,
                });
            }

            foreach (var post in posts)
            {
                linkedPost.Add(new CommentPost()
                {
                    ReplyToPostId = post.Id,
                    CommentId = insertedComment.Entity.Id,
                });
            }

            await _context.AddRangeAsync(linkedComments);
            await _context.AddRangeAsync(linkedPost);

            await _context.SaveChangesAsync();

            return insertedComment.Entity;
        }

        public async Task<Comment> EditComment(Comment commentModel, Guid editorUser)
        {
            var comment = await _context.Comment.FirstOrDefaultAsync(x => x.Id == commentModel.Id);

            if (comment == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.CommentNotFound, $"Comment {commentModel.Id} doesn't exist");

            comment.Text = commentModel.Text;

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
        public async Task<PagedResult<Comment>> GetPageComments(int postId, int page, int quantity)
        {
            if (page <= 0 || quantity <= 0)
                throw ExceptionFactory.SoftException(ExceptionEnum.InappropriatParameters,
                    "inappropriate parameters page or quantity");

            /*return await _context.Comment.Where(x => x.PostId == postId && x.IsArchived == false)
                .Include(x => x.AttachmentComment)
                    .ThenInclude(x => x.Attachment)
                .AsQueryable().GetPaged(page, quantity);*/

            var res = await _context.Comment.Where(x => x.CommentPost.Any(xx => xx.ReplyToPostId == postId))
                .Include(x => x.CommentPost)
                .Include(x => x.AttachmentComment)
                    .ThenInclude(x => x.Attachment)
                .AsQueryable().GetPaged(page, quantity);


            return res;
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