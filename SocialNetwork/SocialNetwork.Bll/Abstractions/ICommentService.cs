using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace SocialNetwork.Bll.Abstractions
{
    public interface ICommentService
    {
        Task<Comment> GetComment(BigInteger commentId);

        Task<Comment> AddComment(Comment commentModel, Guid authorUser);

        Task<Comment> EditComment(Comment commentModel, Guid editorUser);

        Task<PagedResult<Comment>> GetPageComments(BigInteger postId, int page, int quantity);

        Task DeleteComment(BigInteger commentId, Guid userId);
    }
}
