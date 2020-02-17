using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Dal.ViewModels.In
{

    public class PostAttachmentViewModel
    {
        [Required]
        public Guid PostId { get; set; }
        public IFormFile UploadFile { get; set; }
    }

    public class CommentAttachmentViewModel
    {
        [Required]
        public Guid CommentId { get; set; }
        public IFormFile UploadFile { get; set; }
    }
}
