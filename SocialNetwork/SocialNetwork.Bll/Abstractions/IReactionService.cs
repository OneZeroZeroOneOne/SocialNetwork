using SocialNetwork.Dal.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using SocialNetwork.Dal.Extensions;
using SocialNetwork.Dal.ViewModels.In;

namespace SocialNetwork.Bll.Abstractions
{
    
    public interface IReactionService
    {
        Task<ReactionPost> AddReactionPost(ReactionPost reactionPost, User authorUser);

        Task<ReactionComment> AddReactionComment(ReactionComment reactionComment, User authorUser);

        Task<List<ReactionPost>> GetReactionPost(Guid postId);

        Task<List<ReactionComment>> GetReactionComment(Guid commentId);
    }
}
