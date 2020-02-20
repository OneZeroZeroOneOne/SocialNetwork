using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Text;

namespace SocialNetwork.Dal.ViewModels.In
{
    public class EditCommentViewModel
    {
        [Required]
        public int CommentId { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
