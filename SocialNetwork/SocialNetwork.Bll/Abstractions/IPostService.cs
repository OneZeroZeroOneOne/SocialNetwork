using SocialNetwork.Dal.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using SocialNetwork.Dal.Extensions;

namespace SocialNetwork.Bll.Abstractions
{
    public interface IPostService
    {
        Task<Post> EditPost(Post postModel, User editorUser);

        Task<Post> CreateNewPost(Post postModel, User authorUser);

        Task<Post> GetPost(Guid postId);

        Task<PagedQuery.PagedResult<Post>> GetPagePosts(Guid userId, int page, int quantity);
    }
}
