using SocialNetwork.Dal.Models;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace SocialNetwork.Bll.Abstractions
{

    public interface IReactionService
    {
        Task<ReactionPost> AddReactionPost(ReactionPost reactionPost, Guid authorUser);

        Task<ReactionComment> AddReactionComment(ReactionComment reactionComment, Guid authorUser);

        Task<List<ReactionPost>> GetReactionPost(BigInteger postId);

        Task<List<ReactionComment>> GetReactionComment(BigInteger commentId);

        Task<List<ReactionTypePost>> GetReactionTypePost();
        Task<List<ReactionTypeComment>> GetReactionTypeComment();

        Task DeleteReactionPost(BigInteger postId, Guid currentUserId);
        Task DeleteReactionComment(BigInteger commentId, Guid currentUserId);


    }
}
