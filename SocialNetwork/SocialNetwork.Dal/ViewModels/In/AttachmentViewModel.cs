using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using SocialNetwork.Utilities.Attributes;

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
        [Required(ErrorMessage = "Please provide a file.")]
        [DataType(DataType.Upload)]
        [MaxFileSize(5 * 1024 * 1024)]
        [AllowedExtensions(new[] { ".jpg", ".png", ".gif" })]
        public IFormFile UploadFile { get; set; }
    }
}
