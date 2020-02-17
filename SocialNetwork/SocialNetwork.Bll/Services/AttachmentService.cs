using Microsoft.EntityFrameworkCore;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels.In;
using SocialNetwork.Dal.ViewModels.Out;
using SocialNetwork.Utilities.Exceptions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SocialNetwork.Bll.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly PublicContext _publicContext;

        public AttachmentService(PublicContext publicContext)
        {
            _publicContext = publicContext;
        }

        public async Task<OutAttachmentViewModel> SaveAttachmentPost(PostAttachmentViewModel postAttachment, string rootPath)
        {
            if (postAttachment.UploadFile == null)
                ExceptionFactory.SoftException(ExceptionEnum.ProvideFile, "Need to provide file");

            var post = await _publicContext.Post.FirstOrDefaultAsync(x => x.Id == postAttachment.PostId);
            if (post == null)
                ExceptionFactory.SoftException(ExceptionEnum.CommentNotFound, "Comment not found");

            var path = "/Files/" + postAttachment.UploadFile.FileName;
            await using (var fileStream = new FileStream(rootPath + @"\wwwroot\" + path, FileMode.Create))
            {
                await postAttachment.UploadFile.CopyToAsync(fileStream);
            }

            var attachment = new Attachment()
            {
                Id = Guid.NewGuid(),
                ContentType = "image",
                Path = path,
                CreateDateTime = DateTime.UtcNow,
            };

            AttachmentPost attachmentToPost = new AttachmentPost()
            {
                AttachmentId = attachment.Id,
                PostId = postAttachment.PostId,
            };

            await _publicContext.Attachment.AddAsync(attachment);
            await _publicContext.AttachmentPost.AddAsync(attachmentToPost);
            await _publicContext.SaveChangesAsync();

            return new OutAttachmentViewModel { Path = path };
        }

        public async Task<OutAttachmentViewModel> SaveAttachmentComment(CommentAttachmentViewModel commentAttachment, string rootPath)
        {
            if (commentAttachment.UploadFile == null)
                ExceptionFactory.SoftException(ExceptionEnum.ProvideFile, "Need to provide file");

            var comment = await _publicContext.Comment.FirstOrDefaultAsync(x => x.Id == commentAttachment.CommentId);
            if (comment == null)
                ExceptionFactory.SoftException(ExceptionEnum.CommentNotFound, "Comment not found");

            var path = "/Files/" + commentAttachment.UploadFile.FileName;
            await using (var fileStream = new FileStream(rootPath + @"\wwwroot\" + path, FileMode.Create))
            {
                await commentAttachment.UploadFile.CopyToAsync(fileStream);
            }

            var attachment = new Attachment()
            {
                Id = Guid.NewGuid(),
                ContentType = "image",
                Path = path,
                CreateDateTime = DateTime.UtcNow,
            };

            AttachmentComment attachmentToComment = new AttachmentComment()
            {
                AttachmentId = attachment.Id,
                CommentId = commentAttachment.CommentId,
            };

            await _publicContext.Attachment.AddAsync(attachment);
            await _publicContext.AttachmentComment.AddAsync(attachmentToComment);
            await _publicContext.SaveChangesAsync();

            return new OutAttachmentViewModel { Path = path };
        }
    }
}
