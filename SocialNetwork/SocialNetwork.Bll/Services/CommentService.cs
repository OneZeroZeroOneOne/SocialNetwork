using Microsoft.EntityFrameworkCore;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Extensions;
using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels;
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
            var post = await _context.Post.FirstOrDefaultAsync(x => x.Id == commentModel.PostId);

            if (post == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.PostNotFound, $"Post {commentModel.PostId} doesn't exist");

            commentModel.UserId = authorUser;

            post.Comments.Add(commentModel);

            var insertedPost = _context.Update(post);

            await _context.SaveChangesAsync();

            foreach (var attachmentId in attachmentIdList)
            {
                await AttachFileToComment(insertedPost.Entity.Comments.First().Id, attachmentId);
            }

            await _context.SaveChangesAsync();

            return await GetComment(insertedPost.Entity.Comments.First().Id);
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

            return await _context.Comment.Where(x => x.PostId == postId && x.IsArchived == false)
                .Include(x => x.AttachmentComment)
                    .ThenInclude(x => x.Attachment)
                .AsQueryable().GetPaged(page, quantity);
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