using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Exceptions;
using SocialNetwork.Dal.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace SocialNetwork.Bll.Services
{

    public class PostService : IPostService
    {
        private readonly PublicContext _context;
        public PostService(PublicContext publicContext)
        {
            _context = publicContext;
        }

        public async Task<Post> EditPost(Post postModel, User editorUser)
        {
            var postInDb = await _context.Post.FirstOrDefaultAsync(x => x.Id == postModel.Id);

            if (postInDb == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.PostNotFound, $"Post {postModel.Id} not found");

            postInDb.Text = postModel.Text;

            _context.Update(postInDb);
            await _context.SaveChangesAsync();

            return postInDb;
        }

        public async Task<Post> CreateNewPost(Post postModel, User authorUser)
        {
            if (authorUser == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.UserNotFound, $"User {postModel.UserId} doesn't exist");

            postModel.UserId = authorUser.Id;

            var insertedPost = await _context.Post.AddAsync(postModel);
            await _context.SaveChangesAsync();

            return insertedPost.Entity;
        }

        public async Task<Post> GetPost(Guid postId)
        {
            var post = await _context.Post.Where(x => x.Id == postId)
                .Include(x => x.Comments)
                .FirstOrDefaultAsync();

            if (post == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.PostNotFound, $"Post {postId} not found");

            return post;
        }

        public async Task<List<Comment>> GetPageComments(Post post, int page, int quantity)
        {
            List<Comment> comments = await _context.Comment.Where(x => x.PostId == post.Id)
                    .OrderBy(x => x.Date)
                    .Skip(page * (quantity))
                    .Take(quantity)
                    .ToListAsync();
            return comments;
        }
    }
}