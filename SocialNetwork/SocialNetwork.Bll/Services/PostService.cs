using Microsoft.EntityFrameworkCore;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Extensions;
using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels;
using SocialNetwork.Utilities.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Bll.Services
{
    public class PostService : IPostService
    {
        private readonly PublicContext _context;
        public PostService(PublicContext publicContext)
        {
            _context = publicContext;
        }

        public async Task<Post> EditPost(Post postModel, Guid editorUser)
        {
            var postInDb = await _context.Post.FirstOrDefaultAsync(x => x.Id == postModel.Id);

            if (postInDb == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.PostNotFound, $"Post {postModel.Id} not found");

            postInDb.Text = postModel.Text;

            _context.Update(postInDb);
            await _context.SaveChangesAsync();

            return postInDb;
        }

        public async Task<Post> CreateNewPost(Post postModel, Guid authorUser)
        {
            postModel.UserId = authorUser;

            var insertedPost = await _context.Post.AddAsync(postModel);
            await _context.SaveChangesAsync();

            return insertedPost.Entity;
        }

        public async Task<Post> GetPost(Guid postId)
        {
            var post = await _context.Post.FirstOrDefaultAsync(x => x.Id == postId && x.IsArchived == false);

            if (post == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.PostNotFound, $"Post {postId} not found");

            return post;
        }

        public async Task<PagedResult<Post>> GetPagePosts(int page, int quantity)
        {
            if (page <= 0 || quantity <= 0)
                throw ExceptionFactory.SoftException(ExceptionEnum.InappropriatParameters,
                    $"inappropriate parameters page or quantity");

            return await _context.Post.Where(x => x.IsArchived == false).AsQueryable().GetPaged(page, quantity);
        }

        public async Task DeletePost(Guid postId, Guid currentUserId)
        {

            var post = await _context.Post.FirstOrDefaultAsync(x => x.Id == postId && x.UserId == currentUserId);
            if (post == null)
            {
                throw ExceptionFactory.SoftException(ExceptionEnum.PostNotFound,
                    $"Post with {postId} post id not exist");
            }
            post.IsArchived = true;
            _context.Post.Update(post);
            await _context.SaveChangesAsync();
        }
    }
}