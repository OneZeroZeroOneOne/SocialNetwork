using System;
using System.Collections.Generic;
using System.Text;
using SocialNetwork.Dal.Models;

namespace SocialNetwork.Bll.Abstractions
{
    public interface IPostService
    {
        Post CreateNewPost(Post postModel);

        Post GetPost(Guid postId);
    }
}
