using Microsoft.EntityFrameworkCore;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SocialNetwork.Markdown.Jsonize;
using SocialNetwork.Markdown.Jsonize.Models;

namespace SocialNetwork.Bll.Services
{

    public class CommentService : ICommentService
    {
        private readonly PublicContext _context;
        private readonly IUserInputService _userInputSanitizeService;
        private readonly JsonSerializer _jsonWriter;

        public CommentService(PublicContext publicContext, IUserInputService userInputSanitizeService)
        {
            _context = publicContext;
            _userInputSanitizeService = userInputSanitizeService;

            _jsonWriter = new JsonSerializer
            {
                NullValueHandling = (NullValueHandling)1
            };
        }


        private async Task AttachFileToComment(int commentId, int attachmentId)
        {
            if (await _context.Attachment.AnyAsync(x => x.Id == attachmentId))
            {
                var attachmentComment = new AttachmentComment()
                {
                    AttachmentId = attachmentId,
                    CommentId = commentId
                };

                await _context.AttachmentComment.AddAsync(attachmentComment);
            }
        }

        private List<JsonizeNode> FlatNodes(JsonizeNode rootNode)
        {
            var stack = new List<JsonizeNode>();
            stack.AddRange(rootNode.Children);
            var result = new List<JsonizeNode>();

            while (stack.Count > 0)
            {
                var node = stack.PopAt(0);
                if (node.Children != null)
                {
                    stack.AddRange(node.Children);
                }

                result.Add(node);
            }

            return result;
        }

        public async Task<Comment> AddComment(Comment commentModel, Guid authorUser, List<int> attachmentIdList)
        {
            var post = await _context.Post.FirstOrDefaultAsync(x => x.Id == commentModel.PostId);

            if (post == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.PostNotFound, $"Post {commentModel.PostId} doesn't exist");

            commentModel.UserId = authorUser;

            var markdownText = await _userInputSanitizeService.SanitizeHtml(commentModel.Text);
            var nodes = new Jsonize(markdownText).ParseHtmlToTypedJson().Children.FirstOrDefault();//HtmlToJsonService.HtmlToJson(markdownText).Replace("\r\n", "");

            var flattenedNode = FlatNodes(nodes);

            foreach (var node in flattenedNode)
            {
                // ReSharper disable once StringLiteralTypo
                if (node.Tag == "linktocomponent")
                {
                    var (_, idValue) = node.Attributes.FirstOrDefault(x => x.Key == "id");
                    if (idValue != null)
                    {
                        var linkTo = 0; //0 if idk, 1 to post, 2 to comment;

                        if (!int.TryParse((string) idValue, out var intId))
                            throw ExceptionFactory.SoftException(ExceptionEnum.SomethingWentWrong,
                                "Something went wrong");

                        var linkToComment = await _context.Comment.FirstOrDefaultAsync(x => x.Id == intId);
                        if (linkToComment == null) //first i check is it link to comment because link to comment have bigger chance to appear
                        {
                            var linkToPost = await _context.Post.FirstOrDefaultAsync(x => x.Id == intId);
                            if (linkToPost != null)
                            {
                                node.Attributes.TryAdd("isPost", "true");
                                continue;
                            }

                            node.Attributes.TryAdd("isExist", "false");
                            continue;
                        }
                        node.Attributes.TryAdd("isComment", "true");
                    }
                }
            }

            commentModel.Text = JObject.FromObject(nodes, _jsonWriter).ToString().Replace("\r\n", "");

            post.Comments.Add(commentModel);

            var insertedPost = _context.Update(post);

            await _context.SaveChangesAsync();

            if (attachmentIdList != null)
                foreach (var attachmentId in attachmentIdList)
                {
                    await AttachFileToComment(insertedPost.Entity.Comments.Last().Id, attachmentId);
                }

            await _context.SaveChangesAsync();

            return await GetComment(insertedPost.Entity.Comments.Last().Id);
        }

        public async Task<Comment> EditComment(Comment commentModel, Guid editorUser)
        {
            var comment = await _context.Comment.FirstOrDefaultAsync(x => x.Id == commentModel.Id);

            if (comment == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.CommentNotFound, $"Comment {commentModel.Id} doesn't exist");

            comment.Text = await _userInputSanitizeService.SanitizeHtml(commentModel.Text);

            _context.Comment.Update(comment);
            await _context.SaveChangesAsync();

            return comment;
        }

        public async Task<Comment> GetComment(int commentId)
        {
            var comment = await _context.Comment
                .Include(x => x.AttachmentComment)
                    .ThenInclude(x => x.Attachment)
                .FirstOrDefaultAsync(x => x.Id == commentId && x.IsArchived == false);

            if (comment == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.CommentNotFound, $"Comment {commentId} doesn't exist");

            return comment;
        }
        public async Task<PagedResult<Comment>> GetPageComments(int postId, int page, int quantity)
        {
            if (page <= 0 || quantity <= 0)
                throw ExceptionFactory.SoftException(ExceptionEnum.InappropriatParameters,
                    "inappropriate parameters page or quantity");

            return await _context.Comment.Where(x => x.PostId == postId && x.IsArchived == false)
                .Include(x => x.AttachmentComment)
                    .ThenInclude(x => x.Attachment)
                .AsQueryable().GetPaged(page, quantity);
        }
        public async Task DeleteComment(int commentId, Guid currentUserId)
        {
            var comment = await _context.Comment.FirstOrDefaultAsync(x => x.Id == commentId && x.UserId == currentUserId);
            if (comment == null)
            {
                throw ExceptionFactory.SoftException(ExceptionEnum.CommentNotFound,
                    $"Comment with {commentId} comment id not exist");
            }
            comment.IsArchived = true;
            _context.Comment.Update(comment);
            await _context.SaveChangesAsync();
        }
    }
}