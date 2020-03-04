using Microsoft.EntityFrameworkCore;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels.In;
using SocialNetwork.Utilities.Exceptions;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AngleSharp.Network.Default;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;

namespace SocialNetwork.Bll.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly PublicContext _publicContext;
        private readonly IAttachmentPathProvider _attachmentPathProvider;
        private readonly IPreviewGeneratorService _previewGeneratorService;
        private readonly IHttpContextAccessor _httpContext;

        public AttachmentService(PublicContext publicContext, IAttachmentPathProvider attachmentPathProvider, IPreviewGeneratorService previewGeneratorService, IHttpContextAccessor httpContext)
        {
            _publicContext = publicContext;
            _attachmentPathProvider = attachmentPathProvider;
            _previewGeneratorService = previewGeneratorService;
            _httpContext = httpContext;
        }

        public async Task<Attachment> SaveAttachment(AttachmentViewModel attachment)
        {
            var ext = Path.GetExtension(attachment.UploadFile.FileName);
            var fileName = DateTime.UtcNow.ToFileTimeUtc();
            var havePreview = false;
            Guid hash;
            _httpContext.HttpContext.Request.Headers.TryGetValue("X-Width", out var width);
            _httpContext.HttpContext.Request.Headers.TryGetValue("X-Height", out var height);

            int.TryParse(width, out int widthVal);
            int.TryParse(height, out int heightVal);

            await using (var memoryStream = new MemoryStream())
            {
                await attachment.UploadFile.CopyToAsync(memoryStream);
                using(var hasher = new MD5CryptoServiceProvider())
                    hash = new Guid(hasher.ComputeHash(memoryStream.GetBuffer()));

                var storedAttachment = await _publicContext.Attachment.FirstOrDefaultAsync(s => s.Hash == hash);
                if (storedAttachment != null)
                    return storedAttachment;

                await using (var fileStream = new FileStream(
                    Path.Combine(Path.Combine(_attachmentPathProvider.GetPath(), "Files"), fileName + ext),
                    FileMode.Create))
                {
                    await fileStream.WriteAsync(memoryStream.GetBuffer());
                }
                
            }

            if (attachment.UploadFile.ContentType.Contains("video"))
            {
                havePreview = true;
                await _previewGeneratorService.GeneratePreview(Path.Combine(_attachmentPathProvider.GetPath(), "Files"),
                    fileName.ToString(), ext);
            }

            var attachmentDb = new Attachment
            {
                ContentType = attachment.UploadFile.ContentType,
                Path = "Files/" + fileName + ext,
                Preview = havePreview ? "Files/" + fileName + "_preview.png" : null,
                Hash = hash,
                DisplayName = attachment.UploadFile.FileName,
                Width = widthVal,
                Height = heightVal,
            };

            await _publicContext.Attachment.AddAsync(attachmentDb);
            await _publicContext.SaveChangesAsync();

            return attachmentDb;
        }

        public async Task<bool> AttachmentToPost(PostAttachmentViewModel postAttachment)
        {
            var post = await _publicContext.Post.FirstOrDefaultAsync(x => x.Id == postAttachment.PostId);

            if (post == null)
                ExceptionFactory.SoftException(ExceptionEnum.CommentNotFound, "Comment not found");

            if (!await _publicContext.Attachment.AnyAsync(x => x.Id == postAttachment.AttachmentId))
                ExceptionFactory.SoftException(ExceptionEnum.AttachmentNotFound,
                    $"Attachment {postAttachment.AttachmentId} not found");

            var attachmentToPost = new AttachmentPost()
            {
                AttachmentId = postAttachment.AttachmentId,
                PostId = postAttachment.PostId,
            };

            await _publicContext.AttachmentPost.AddAsync(attachmentToPost);
            await _publicContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AttachmentToComment(CommentAttachmentViewModel commentAttachment)
        {
            var comment = await _publicContext.Comment.FirstOrDefaultAsync(x => x.Id == commentAttachment.CommentId);

            if (comment == null)
                ExceptionFactory.SoftException(ExceptionEnum.CommentNotFound, "Comment not found");

            if (!await _publicContext.Attachment.AnyAsync(x => x.Id == commentAttachment.AttachmentId))
                ExceptionFactory.SoftException(ExceptionEnum.AttachmentNotFound,
                    $"Attachment {commentAttachment.AttachmentId} not found");

            var attachmentToComment = new AttachmentComment()
            {
                AttachmentId = commentAttachment.AttachmentId,
                CommentId = commentAttachment.CommentId,
            };

            await _publicContext.AttachmentComment.AddAsync(attachmentToComment);
            await _publicContext.SaveChangesAsync();

            return true;
        }
    }
}
