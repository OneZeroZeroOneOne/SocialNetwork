using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialNetwork.Bll.Abstractions
{
    public interface ICommentService
    {
        Task<Comment> GetComment(int commentId);

        Task<Comment> AddComment(Comment commentModel, Guid authorUser, List<int> attachmentIdList);

        Task<Comment> EditComment(Comment commentModel, Guid editorUser);

        Task<PagedResult<Comment>> GetPageComments(int postId, int page, int quantity, bool sortOrder = false);

        Task DeleteComment(int commentId, Guid userId);
    }
}
