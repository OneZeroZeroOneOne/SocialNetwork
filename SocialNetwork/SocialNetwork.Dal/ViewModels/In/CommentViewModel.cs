using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SocialNetwork.Dal.ViewModels
{
    public class CommentViewModel
    {
        [Required]
        public string Text { get; set; }
        [Required]
        public Guid PostId { get; set; }
    }
}
