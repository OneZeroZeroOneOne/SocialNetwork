using Microsoft.EntityFrameworkCore;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Exceptions;
using SocialNetwork.Dal.Models;
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

        public async Task<ReactionPost> AddReactionPost(ReactionPost reactionPost, User authorUser)
        {
            if (await _context.ReactionPost.AnyAsync(x => x.PostId == reactionPost.PostId && x.UserId == authorUser.Id))
                    throw ExceptionFactory.SoftException(ExceptionEnum.ReactionAlreadyExist,
                        $"another reaction already  exist");

            if (!await _context.ReactionTypePost.AnyAsync(x => x.ReactionId == reactionPost.ReactionId))
                throw ExceptionFactory.SoftException(ExceptionEnum.ReactionDoesNotExist,
                    $"reaction does not exist");

            reactionPost.UserId = authorUser.Id;
            var insertedReactionPost = await _context.ReactionPost.AddAsync(reactionPost);
            await _context.SaveChangesAsync();
            return insertedReactionPost.Entity;
        }

        public async Task<ReactionComment> AddReactionComment(ReactionComment reactionComment, User authorUser)
        {
            if (await _context.ReactionComment.AnyAsync(x => x.CommentId == reactionComment.CommentId && x.UserId == authorUser.Id))
                throw ExceptionFactory.SoftException(ExceptionEnum.ReactionAlreadyExist,
                    $"another reaction already  exist");

            if (!await _context.ReactionTypeComment.AnyAsync(x => x.ReactionId == reactionComment.ReactionId))
                throw ExceptionFactory.SoftException(ExceptionEnum.ReactionDoesNotExist,
                    $"reactionId does not exist");

            reactionComment.UserId = authorUser.Id;

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
    }
}
