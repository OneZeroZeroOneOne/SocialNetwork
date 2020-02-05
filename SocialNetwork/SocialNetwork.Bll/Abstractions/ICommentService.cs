using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels;
using System;
using System.Threading.Tasks;

namespace SocialNetwork.Bll.Abstractions
{
    public interface ICommentService
    {
        Task<Comment> GetComment(Guid commentId);

        Task<Comment> AddComment(Comment commentModel, User authorUser);

        Task<Comment> EditComment(Comment commentModel, User editorUser);

        Task<PagedResult<Comment>> GetPageComments(Guid postId, int page, int quantity);
    }
}
