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
        Task<PostReaction> AddPostReaction(Post postModel, User editorUser);

        Task<CommentReaction> AddCommentReaction(Post postModel, User authorUser);

        Task<List<PostReaction>> GetPostReaction(Guid postId);

        Task<List<CommentReaction>> GetCommentReaction(Guid commentId);
    }
}
