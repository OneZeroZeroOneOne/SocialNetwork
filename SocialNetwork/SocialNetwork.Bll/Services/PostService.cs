﻿using Microsoft.EntityFrameworkCore;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Extensions;
using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels;
using SocialNetwork.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Bll.Services
{
    public class PostService : IPostService
    {
        private readonly PublicContext _context;
        private readonly IUserInputService _userInputSanitizeService;

        public PostService(PublicContext publicContext, IUserInputService userInputSanitizeService)
        {
            _context = publicContext;
            _userInputSanitizeService = userInputSanitizeService;
        }

        public async Task<Post> EditPost(Post postModel, Guid editorUser)
        {
            var postInDb = await _context.Post.FirstOrDefaultAsync(x => x.Id == postModel.Id);

            if (postInDb == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.PostNotFound, $"Post {postModel.Id} not found");

            postInDb.Title = await _userInputSanitizeService.SanitizeHtml(postModel.Title);
            postInDb.Text = await _userInputSanitizeService.SanitizeHtml(postModel.Text);

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
            if (!_context.Board.Any(x => x.Id == postModel.BoardId))
                throw ExceptionFactory.SoftException(ExceptionEnum.BoardNotFound, $"Board {postModel.BoardId} not found");

            postModel.UserId = authorUser;
            postModel.Text = await _userInputSanitizeService.SanitizeHtml(postModel.Text);
            postModel.Title = await _userInputSanitizeService.SanitizeHtml(postModel.Title);

            var insertedPost = await _context.Post.AddAsync(postModel);
            await _context.SaveChangesAsync();

            if (attachmentPostList != null)
                foreach (var attachmentId in attachmentPostList)
                {
                    await AttachFileToPost(insertedPost.Entity.Id, attachmentId);
                }

            await _context.SaveChangesAsync();

            return await GetPost(postModel.BoardId, postModel.Id);
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
                    $"Inappropriate parameters page or quantity");

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