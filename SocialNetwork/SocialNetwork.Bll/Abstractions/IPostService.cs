using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels;
using System;
using System.Threading.Tasks;

namespace SocialNetwork.Bll.Abstractions
{
    public interface IPostService
    {
        Task<Post> EditPost(Post postModel, Guid editorUser);

        Task<Post> CreateNewPost(Post postModel, Guid authorUser);

        Task<Post> GetPost(Guid postId);

        Task<PagedResult<Post>> GetPagePosts(Guid userId, int page, int quantity);

        Task DeletePost(Guid postId, Guid userId);
    }
}
