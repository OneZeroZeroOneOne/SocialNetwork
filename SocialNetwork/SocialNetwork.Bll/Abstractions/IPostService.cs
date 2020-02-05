using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels;
using System;
using System.Threading.Tasks;

namespace SocialNetwork.Bll.Services;
{
    public interface IPostService
    {
        Task<Post> EditPost(Post postModel, User editorUser);

        Task<Post> CreateNewPost(Post postModel, User authorUser);

        Task<Post> GetPost(Guid postId);

        Task<PagedResult<Post>> GetPagePosts(Guid userId, int page, int quantity);
    }
}
