using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Dal.ViewModels.In
{
    public class CommentViewModel
    {
        [Required]
        public string Text { get; set; }
        [Required]
        public Guid PostId { get; set; }
    }
}
