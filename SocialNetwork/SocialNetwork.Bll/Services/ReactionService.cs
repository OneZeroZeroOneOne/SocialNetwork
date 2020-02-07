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
            if (reactionPost.ReactionId == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.InappropriatParameters,
                    $"reactionPost han no ReactionId");

            reactionPost.UserId = authorUser;
            var insertedReactionPost = await _context.ReactionPost.AddAsync(reactionPost);
            await _context.SaveChangesAsync();

            return insertedReactionPost.Entity;
        }

        public async Task<ReactionComment> AddReactionComment(ReactionComment reactionComment, Guid authorUser)
        {
            if (reactionComment == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.InappropriatParameters,
                    $"inappropriate parameters commentId");

            
            reactionComment.UserId = authorUser;
            var insertedReactionComment = await _context.ReactionComment.AddAsync(reactionComment);
            await _context.SaveChangesAsync();

            return insertedReactionComment.Entity;
        }

        public async Task<List<ReactionPost>> GetReactionPost(Guid postId)
        {
            if (postId == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.InappropriatParameters,
                    $"inappropriate parameters postId");
            
            return await _context.ReactionPost.Where(x => x.PostId == postId).ToListAsync();
        }

        
        public async Task<List<ReactionComment>> GetReactionComment(Guid commentId)
        {
            if (commentId == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.InappropriatParameters,
                    $"inappropriate parameters commentId");
            
            return await _context.ReactionComment.Where(x => x.CommentId == commentId).ToListAsync();
        }
    }
}
