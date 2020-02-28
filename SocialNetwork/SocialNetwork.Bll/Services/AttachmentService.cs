using Microsoft.EntityFrameworkCore;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels.In;
using SocialNetwork.Utilities.Exceptions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SocialNetwork.Bll.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly PublicContext _publicContext;
        private readonly IAttachmentPathProvider _attachmentPathProvider;
        private readonly IPreviewGeneratorService _previewGeneratorService;

        public AttachmentService(PublicContext publicContext, IAttachmentPathProvider attachmentPathProvider, IPreviewGeneratorService previewGeneratorService)
        {
            _publicContext = publicContext;
            _attachmentPathProvider = attachmentPathProvider;
            _previewGeneratorService = previewGeneratorService;
        }

        public async Task<Attachment> SaveAttachment(AttachmentViewModel attachment)
        {
            var ext = Path.GetExtension(attachment.UploadFile.FileName);
            var fileName = DateTime.UtcNow.ToFileTimeUtc();
            var havePreview = false;

            await using (var fileStream = new FileStream(Path.Combine(Path.Combine(_attachmentPathProvider.GetPath(), "Files"), fileName + ext), FileMode.Create))
            {
                await attachment.UploadFile.CopyToAsync(fileStream);
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
