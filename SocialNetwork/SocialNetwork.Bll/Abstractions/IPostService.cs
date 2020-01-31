using SocialNetwork.Dal.Models;
using System;

namespace SocialNetwork.Bll.Abstractions
{
    public interface IPostService
    {
        Post CreateNewPost(Post postModel, User authorUser);

        Post GetPost(Guid postId);
    }
}
