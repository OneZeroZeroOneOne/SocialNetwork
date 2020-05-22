using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels;
using SocialNetwork.RequestLifetimeBll.Abstractions;
using SocialNetwork.Utilities.Exceptions;

namespace SocialNetwork.Bll.Services
{
    public class PostService : IPostService
    {
        private readonly PublicContext _context;
        private readonly IUserInputService _userInputService;
        private readonly IRequestLifetimeService _requestLifetimeService;
        private readonly IHttpContextAccessor _httpContext;

        public PostService(PublicContext publicContext, IUserInputService userInputSanitizeService, IRequestLifetimeService requestLifetimeService, IHttpContextAccessor httpContext)
        {
            _context = publicContext;

            _userInputService = userInputSanitizeService;
            _requestLifetimeService = requestLifetimeService;
            _httpContext = httpContext;
        }

        public async Task<Post> EditPost(Post postModel, Guid editorUser)
        {
            var postInDb = await _context.Post.FirstOrDefaultAsync(x => x.Id == postModel.Id);

            if (postInDb == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.PostNotFound, $"Post {postModel.Id} not found");

            postInDb.Title = await _userInputService.SanitizeHtml(postModel.Title);
            postInDb.Text = await _userInputService.Markdown(postModel.Text);

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
            var board = await _context.Board.FirstOrDefaultAsync(x => x.Id == postModel.BoardId);

            if (board == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.BoardNotFound, $"Board {postModel.BoardId} not found");

            postModel.UserId = authorUser;

            postModel.Title = await _userInputService.SanitizeHtml(postModel.Title);

            var insertedPost = await _context.Post.AddAsync(postModel);
            await _context.SaveChangesAsync();

            if (attachmentPostList != null)
                foreach (var attachmentId in attachmentPostList)
                {
                    await AttachFileToPost(insertedPost.Entity.Id, attachmentId);
                }

            await _context.PostTop.AddAsync(new PostTop
            {
                PostId = postModel.Id,
                Score = 50,
                BoardId = postModel.BoardId
            });
			
            await _context.SaveChangesAsync();

            var createdPost = await GetPost(postModel.BoardId, postModel.Id);

            _requestLifetimeService.SetPost(createdPost);
            _requestLifetimeService.SetBoard(board);

            _httpContext.HttpContext.Items.Add("RequestLifetime", _requestLifetimeService);

            createdPost.Text = await _userInputService.Markdown(postModel.Text);

            await _context.SaveChangesAsync();

            return createdPost;
        }

        public async Task<Post> GetPostGlobal(int postId)
        {
            var post = await _context.Post
                .Include(x => x.AttachmentPost)
                .ThenInclude(x => x.Attachment)
                .FirstOrDefaultAsync(x => x.Id == postId && x.IsArchived == false);

            if (post == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.PostNotFound, $"Post {postId} not found");

            return post;
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
                    "Post and quantity must be real numbers");

            /*var builder = new FilterDefinitionBuilder<PostTop>();
            var filter = builder.Empty; // select all documents
            filter &= builder.Where(x => x.BoardId == boardId);

            var posts = await _noSqlContext.Collection<PostTop>()
                .Find(filter)
                .Limit(quantity)
                .Skip((page - 1) * quantity)
                .SortByDescending(x => x.Score)
                .Project(x => new List<int>()
                {
                    x.PostId,
                })
                .FirstOrDefaultAsync();*/

            var query = _context.Post
                .Include(x => x.AttachmentPost)
                    .ThenInclude(x => x.Attachment)
                .Join(_context.PostTop, post => post.Id, top => top.PostId, (post, top) =>
                    new
                    {
                        post.Id,
                        post.Date,
                        post.Text,
                        post.Title,
                        post.UserId,
                        post.BoardId,
                        post.IsArchived,
                        post.AttachmentPost,
                        top.Score,
                    })
                .Where(x => x.BoardId == boardId)
                .OrderByDescending(x => x.Score)
                .Select(x => new Post
                {
                    Id = x.Id,
                    BoardId = x.BoardId,
                    Date = x.Date,
                    Title = x.Title,
                    Text = x.Text,
                    IsArchived = x.IsArchived,
                    UserId = x.UserId,
                    AttachmentPost = x.AttachmentPost,
                });

            var postCount = await query.CountAsync();

            var thisPage = new PagedResult<Post>
            {
                CurrentPage = page,
                PageSize = quantity,
                RowCount = postCount,
                Results = await query.ToListAsync(),
                PageCount = (int)Math.Ceiling((double)postCount / quantity),
            };

            return thisPage; /*Where(x => x.IsArchived == false && x.BoardId == boardId)
                .Include(x => x.AttachmentPost)
                    .ThenInclude(x => x.Attachment)
                .AsQueryable().GetPaged(page, quantity);*/
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