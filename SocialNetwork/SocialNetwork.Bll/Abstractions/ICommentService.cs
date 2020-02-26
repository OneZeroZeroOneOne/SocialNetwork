using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels;
using System;
using System.Numerics;
using System.Threading.Tasks;
using SocialNetwork.Dal.ViewModels.In;

namespace SocialNetwork.Bll.Abstractions
{
    public interface ICommentService
    {
        Task<Comment> GetComment(int commentId);

        Task<Comment> AddComment(CommentViewModel commentModel, Guid authorUser);

        Task<Comment> EditComment(Comment commentModel, Guid editorUser);

        Task<PagedResult<Comment>> GetPageComments(int postId, int page, int quantity);

        Task DeleteComment(int commentId, Guid userId);
    }
}
