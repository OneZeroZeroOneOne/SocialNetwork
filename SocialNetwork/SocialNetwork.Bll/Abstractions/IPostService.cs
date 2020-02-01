using SocialNetwork.Dal.Models;
using System;
using System.Threading.Tasks;

namespace SocialNetwork.Bll.Abstractions
{
    public interface IPostService
    {
        Task<Post> EditPost(Post postModel, User editorUser);

        Task<Post> CreateNewPost(Post postModel, User authorUser);

        Task<Post> GetPost(Guid postId);
    }
}
