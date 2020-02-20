using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace SocialNetwork.Bll.Abstractions
{
    public interface IPostService
    {
        Task<Post> EditPost(Post postModel, Guid editorUser);

        Task<Post> CreateNewPost(Post postModel, Guid authorUser);

        Task<Post> GetPost(Guid boardId, BigInteger postId);

        Task<PagedResult<Post>> GetPagePosts(Guid boardId, int page, int quantity);

        Task DeletePost(Guid boardId, BigInteger postId, Guid userId);
    }
}
