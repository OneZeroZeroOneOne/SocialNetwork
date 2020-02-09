using Microsoft.EntityFrameworkCore;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Models;
using SocialNetwork.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Bll.Services
{
    public class ReactionService : IReactionService
    {
        private readonly PublicContext _context;
        public ReactionService(PublicContext publicContext)
        {
            _context = publicContext;
        }

        public async Task<ReactionPost> AddReactionPost(ReactionPost reactionPost, Guid authorUser)
        {
            if (await _context.ReactionPost.AnyAsync(x => x.PostId == reactionPost.PostId && x.UserId == authorUser))
                    throw ExceptionFactory.SoftException(ExceptionEnum.ReactionAlreadyExist,
                        $"another reaction already  exist");

            if (!await _context.ReactionTypePost.AnyAsync(x => x.ReactionId == reactionPost.ReactionId))
                throw ExceptionFactory.SoftException(ExceptionEnum.ReactionDoesNotExist,
                    $"reaction does not exist");

            reactionPost.UserId = authorUser;
            var insertedReactionPost = await _context.ReactionPost.AddAsync(reactionPost);
            await _context.SaveChangesAsync();
            return insertedReactionPost.Entity;
        }

        public async Task<ReactionComment> AddReactionComment(ReactionComment reactionComment, Guid authorUser)
        {
            if (await _context.ReactionComment.AnyAsync(x => x.CommentId == reactionComment.CommentId && x.UserId == authorUser))
                throw ExceptionFactory.SoftException(ExceptionEnum.ReactionAlreadyExist,
                    $"another reaction already  exist");

            if (!await _context.ReactionTypeComment.AnyAsync(x => x.ReactionId == reactionComment.ReactionId))
                throw ExceptionFactory.SoftException(ExceptionEnum.ReactionDoesNotExist,
                    $"reactionId does not exist");

            reactionComment.UserId = authorUser;
            var insertedReactionComment = await _context.ReactionComment.AddAsync(reactionComment);

            await _context.SaveChangesAsync();
            return insertedReactionComment.Entity;
        }

        public async Task<List<ReactionPost>> GetReactionPost(Guid postId)
        {
            var post = await _context.Post.Where(x => x.Id == postId)
                .Include(x => x.ReactionPost).FirstOrDefaultAsync();

            if (post == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.PostNotFound,
                    $"Post with {postId} post id not found");

            return post.ReactionPost.ToList();
        }

        
        public async Task<List<ReactionComment>> GetReactionComment(Guid commentId)
        {
            var comment = await _context.Comment.Where(x => x.Id == commentId)
                .Include(x => x.ReactionComment).FirstOrDefaultAsync();

            if (comment == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.CommentNotFound,
                    $"Comment with {commentId} comment id not exist");

            return comment.ReactionComment.ToList();
        }

        public Task<List<ReactionTypePost>> GetReactionTypePost()
        {
            return _context.ReactionTypePost.ToListAsync();
        }

        public Task<List<ReactionTypeComment>> GetReactionTypeComment()
        {
            return _context.ReactionTypeComment.ToListAsync();
        }

        public void DeleteReactionPost(Guid postId, Guid currentUserId)
        {
            ReactionPost reactionPost = new ReactionPost();
            reactionPost.PostId = postId;
            reactionPost.UserId = currentUserId;
            _context.ReactionPost.Remove(reactionPost);
            
        }

        public void DeleteReactionComment(Guid commentId, Guid currentUserId)
        {
            ReactionComment reactionComment = new ReactionComment();
            reactionComment.CommentId = commentId;
            reactionComment.UserId = currentUserId;
            _context.ReactionComment.Remove(reactionComment);
        }
    }
}
