using SocialNetwork.Dal.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using SocialNetwork.Dal.Extensions;


namespace SocialNetwork.Bll.Services
{
    
    public interface IReactionService
    {
        Task<PostReaction> AddPostReaction(Post postModel, User editorUser);

        Task<CommentReaction> AddCommentReaction(Post postModel, User authorUser);

        Task<PostReaction> GetPostReaction(Guid postId);

        Task<CommentReaction> GetCommentReaction(Guid postId);
    }
}
