using Microsoft.EntityFrameworkCore;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Exceptions;
using SocialNetwork.Dal.Extensions;
using SocialNetwork.Dal.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using SocialNetwork.Dal.ViewModels.In;
using System.Collections.Generic;

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
            if (reactionPost.ReactionId == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.InappropriatParameters,
                    $"reactionPost han no ReactionId");

            reactionPost.UserId = authorUser.Id;
            var insertedReactionPost = await _context.ReactionPost.AddAsync(reactionPost);
            await _context.SaveChangesAsync();

            return insertedReactionPost.Entity;
        }

        public async Task<ReactionComment> AddReactionComment(ReactionComment reactionComment, User authorUser)
        {
            if (reactionComment == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.InappropriatParameters,
                    $"inappropriate parameters commentId");

            
            reactionComment.UserId = authorUser.Id;
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
