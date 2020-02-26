using Microsoft.EntityFrameworkCore;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Extensions;
using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels;
using SocialNetwork.Utilities.Exceptions;
using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Internal;

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

        private async Task AttachFileToPost(int postId, int attachmentId)
        {
            if (await _context.Attachment.AnyAsync(x => x.Id == attachmentId))
            {
                var attachmentPost = new AttachmentPost
                {
                    AttachmentId = attachmentId, PostId = postId
                };

                await _context.AttachmentPost.AddAsync(attachmentPost);
            }
        }

        public async Task<Post> CreateNewPost(Post postModel, Guid authorUser, List<int> attachmentPostList)
        {
            postModel.UserId = authorUser;

            var insertedPost = await _context.Post.AddAsync(postModel);
            _context.SaveChanges();

            foreach (var attachmentId in attachmentPostList)
            {
                await AttachFileToPost(insertedPost.Entity.Id, attachmentId);
            }

            await _context.SaveChangesAsync();

            return await GetPost(postModel.BoardId, postModel.Id);
        }

        public async Task<Post> GetPost(Guid boardId, int postId)
        {
            var post = await _context.Post
                .Include(x => x.AttachmentPost)
                    .ThenInclude(x => x.Attachment)
                .FirstOrDefaultAsync(x => x.Id == postId && x.IsArchived == false && x.BoardId == boardId);

            if (post == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.PostNotFound, $"Post {postId} not found");

            return post;
        }

        public async Task<PagedResult<Post>> GetPagePosts(Guid boardId, int page, int quantity)
        {
            if (page <= 0 || quantity <= 0)
                throw ExceptionFactory.SoftException(ExceptionEnum.InappropriatParameters,
                    $"inappropriate parameters page or quantity");

            return await _context.Post.Where(x => x.IsArchived == false && x.BoardId == boardId)
                .Include(x => x.AttachmentPost)
                    .ThenInclude(x => x.Attachment)
                .AsQueryable().GetPaged(page, quantity);
        }

        public async Task DeletePost(Guid boardId, int postId, Guid currentUserId)
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