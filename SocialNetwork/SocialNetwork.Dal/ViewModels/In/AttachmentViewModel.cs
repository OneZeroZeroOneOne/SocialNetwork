using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using SocialNetwork.Dal.Attributes;

namespace SocialNetwork.Dal.ViewModels.In
{

    public class PostAttachmentViewModel
    {
        [Required]
        public int PostId { get; set; }
        [Required]
        public int AttachmentId { get; set; }
    }

    public class CommentAttachmentViewModel
    {
        [Required]
        public int CommentId { get; set; }
        [Required]
        public int AttachmentId { get; set; }
    }

    public class AttachmentViewModel
    {
        [Required(ErrorMessage = "Please provide a file.")]
        [DataType(DataType.Upload)]
        [MaxFileSize(5 * 1024 * 1024)]
        [AllowedExtensions(new[] { ".jpg", ".png", ".gif", ".mp4", ".webm" })]
        public IFormFile UploadFile { get; set; }
    }
}
