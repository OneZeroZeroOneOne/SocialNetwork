using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Dal.ViewModels.In
{

    public class PostAttachmentViewModel
    {
        [Required]
        public Guid PostId { get; set; }
        [Required]
        public Guid AttachmentId { get; set; }
    }

    public class CommentAttachmentViewModel
    {
        [Required]
        public Guid CommentId { get; set; }
        [Required]
        public Guid AttachmentId { get; set; }
    }

    public class AttachmentViewModel
    {
        public IFormFile UploadFile { get; set; }
    }
}
