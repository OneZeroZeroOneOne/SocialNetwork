using System;
using System.Linq;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Exceptions;
using SocialNetwork.Dal.Models;

namespace SocialNetwork.Bll.Services
{
    public class PostService : IPostService
    {
        private readonly PublicContext _context;
        public PostService(PublicContext publicContext)
        {
            _context = publicContext;
        }

        public Post CreateNewPost(Post postModel)
        {
            //создаем пост, но проверяем существует ли юзер от которого хотят создать пост в базе, иначе будет ошибка
            var user = _context.User.FirstOrDefault(x => x.Id == postModel.UserId);
            if (user == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.UserNotFound, $"User {postModel.UserId} doesn't exist");

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