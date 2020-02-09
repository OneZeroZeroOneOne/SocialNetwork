using SocialNetwork.Dal.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialNetwork.Bll.Abstractions
{

    public interface IReactionService
    {
        Task<ReactionPost> AddReactionPost(ReactionPost reactionPost, Guid authorUser);

        Task<ReactionComment> AddReactionComment(ReactionComment reactionComment, Guid authorUser);

        Task<List<ReactionPost>> GetReactionPost(Guid postId);

        Task<List<ReactionComment>> GetReactionComment(Guid commentId);

        Task<List<ReactionTypePost>> GetReactionTypePost();
        Task<List<ReactionTypeComment>> GetReactionTypeComment();

        void DeleteReactionPost(Guid postId, Guid currentUserId);
        void DeleteReactionComment(Guid commentId, Guid currentUserId);


    }
}
