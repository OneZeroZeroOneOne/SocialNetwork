using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SocialNetwork.Dal.ViewModels.In
{
    public class EditCommentViewModel
    {
        [Required]
        public Guid CommentId { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
