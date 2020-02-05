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

        [Authorize(Roles = "Member")]
        public async Task<PostReaction> AddPostReaction(Post postModel, User editorUser)
        {

        }

        public async Task<ReactionComment> AddCommentReaction(ReactionComment reactionComment, User authorUser)
        {
            if(reactionComment = )
            reactionComment.UserId = authorUser.Id;
            var insertedReactionComment = await _context.ReactionComment.AddAsync(reactionComment);
            await _context.SaveChangesAsync();

            return insertedReactionComment.Entity;


        }

        public async Task<List<ReactionPost>> GetPostReaction(Guid postId)
        {
            if (postId != null)
            {
                throw ExceptionFactory.SoftException(ExceptionEnum.InappropriatParameters,
                    $"inappropriate parameters postId");
            }
            return await _context.ReactionPost.Where(x => x.PostId == postId).ToListAsync();

        }

        
        public async Task<List<ReactionComment>> GetCommentReaction(Guid commentId)
        {
            if (commentId != null)
            {
                throw ExceptionFactory.SoftException(ExceptionEnum.InappropriatParameters,
                    $"inappropriate parameters commentId");
            }
            return await _context.ReactionComment.Where(x => x.CommentId == commentId).ToListAsync();

        }
    }
}
