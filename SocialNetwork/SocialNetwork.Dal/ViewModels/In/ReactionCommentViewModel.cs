using System;
using System.ComponentModel.DataAnnotations;


namespace SocialNetwork.Dal.ViewModels.In
{
    public class ReactionCommentViewModel
    {
        [Required]
        public string CommentId { get; set; }
        public string ReactionId { get; set; }
    }
}
