using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Dal.Models;

namespace SocialNetwork.Bll.Abstractions
{
    public interface ICommentService
    {
        Task<Comment> GetComment(Guid commentId);

        Task<Comment> AddComment(Comment commentModel, User authorUser);

        Task<Comment> EditComment(Comment commentModel, User editorUser);
    }
}
