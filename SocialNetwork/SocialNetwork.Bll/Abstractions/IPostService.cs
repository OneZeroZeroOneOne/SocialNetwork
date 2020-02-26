using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels;
using System;
using System.Numerics;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace SocialNetwork.Bll.Abstractions
{
    public interface IPostService
    {
        Task<Post> EditPost(Post postModel, Guid editorUser);

        Task<Post> CreateNewPost(Post postModel, Guid authorUser, List<int> attachmentPosts);

        Task<Post> GetPost(Guid boardId, int postId);

        Task<PagedResult<Post>> GetPagePosts(Guid boardId, int page, int quantity);

        Task DeletePost(Guid boardId, int postId, Guid userId);
    }
}
