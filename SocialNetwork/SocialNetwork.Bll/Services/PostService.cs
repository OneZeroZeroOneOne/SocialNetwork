using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Exceptions;
using SocialNetwork.Dal.Models;
using System;
using System.Linq;

namespace SocialNetwork.Bll.Services
{

    public class PostService : IPostService
    {
        private readonly PublicContext _context;
        public PostService(PublicContext publicContext)
        {
            _context = publicContext;
        }

        public Post CreateNewPost(Post postModel, User authorUser)
        {
            if (authorUser == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.UserNotFound, $"User {postModel.UserId} doesn't exist");

            postModel.UserId = authorUser.Id;

            var insertedPost = _context.Post.Add(postModel);
            _context.SaveChanges();

            return insertedPost.Entity;
        }

        public Post GetPost(Guid postId)
        {
            var post = _context.Post.FirstOrDefault(x => x.Id == postId);

            return post;
        }
    }
}