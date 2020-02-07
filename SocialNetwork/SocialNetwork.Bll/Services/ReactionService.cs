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
            Console.WriteLine(reactionPost.ReactionId);
            if (await _context.ReactionTypePost.AnyAsync(x => x.ReactionId == reactionPost.ReactionId))
            {
                Console.WriteLine(reactionPost.ReactionId);
                reactionPost.UserId = authorUser.Id;
                var insertedReactionPost = await _context.ReactionPost.AddAsync(reactionPost);
                await _context.SaveChangesAsync();
                return insertedReactionPost.Entity;
            }
            throw ExceptionFactory.SoftException(ExceptionEnum.ReactionDoesNotExist,
                    $"reaction does not exist");
        }

        public async Task<ReactionComment> AddReactionComment(ReactionComment reactionComment, User authorUser)
        {
            Console.WriteLine(reactionComment.ReactionId);
            if (await _context.ReactionTypeComment.AnyAsync(x => x.ReactionId == reactionComment.ReactionId))
            {
                Console.WriteLine(reactionComment.ReactionId);
                reactionComment.UserId = authorUser.Id;
                var insertedReactionComment = await _context.ReactionComment.AddAsync(reactionComment);
                await _context.SaveChangesAsync();
                return insertedReactionComment.Entity;
            }
            throw ExceptionFactory.SoftException(ExceptionEnum.ReactionDoesNotExist,
                    $"reaction does not exist");

        }

        public async Task<List<ReactionPost>> GetReactionPost(Guid postId)
        {
            if (postId == null)
            {
                throw ExceptionFactory.SoftException(ExceptionEnum.InappropriatParameters,
                    $"inappropriate parameters postId");
            }
            var post = await _context.ReactionPost.Where(x => x.PostId == postId).ToListAsync();
            if (post == null)
            {
                throw ExceptionFactory.SoftException(ExceptionEnum.PostNotFound,
                    $"post not found");
            }
            return post;

        }

        
        public async Task<List<ReactionComment>> GetReactionComment(Guid commentId)
        {
            if (commentId == null)
            {
                throw ExceptionFactory.SoftException(ExceptionEnum.InappropriatParameters,
                    $"inappropriate parameters commentId");
            }
            var comment = await _context.ReactionComment.Where(x => x.CommentId == commentId).ToListAsync();
            if (comment == null)
            {
                throw ExceptionFactory.SoftException(ExceptionEnum.PostNotFound,
                    $"comment not found");
            }
            return comment;

        }
    }
}
