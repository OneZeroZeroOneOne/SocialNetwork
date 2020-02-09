using Microsoft.EntityFrameworkCore;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Extensions;
using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using SocialNetwork.Utilities.Exceptions;

namespace SocialNetwork.Bll.Services
{

    public class CommentService : ICommentService
    {
        private readonly PublicContext _context;
        public CommentService(PublicContext publicContext)
        {
            _context = publicContext;
        }

        public async Task<Comment> AddComment(Comment commentModel, Guid authorUser)
        {
            var post = await _context.Post.FirstOrDefaultAsync(x => x.Id == commentModel.PostId);

            if (post == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.PostNotFound, $"Post {commentModel.PostId} doesn't exist");

            commentModel.UserId = authorUser;

            post.Comments.Add(commentModel);

            _context.Update(post);
            await _context.SaveChangesAsync();

            return commentModel;
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

        public async Task<Comment> GetComment(Guid commentId)
        {
            var comment = await _context.Comment.FirstOrDefaultAsync(x => x.Id == commentId && x.IsArchived == false);

            if (comment == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.CommentNotFound, $"Comment {commentId} doesn't exist");

            return comment;
        }
        public async Task<PagedResult<Comment>> GetPageComments(Guid postId, int page, int quantity)
        {
            if (page <= 0 || quantity <= 0)
                throw ExceptionFactory.SoftException(ExceptionEnum.InappropriatParameters,
                    "inappropriate parameters page or quantity");

            return await _context.Comment.Where(x => x.PostId == postId && x.IsArchived == false).AsQueryable().GetPaged(page, quantity);
        }

    }
}