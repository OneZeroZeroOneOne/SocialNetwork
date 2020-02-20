using System;
using System.ComponentModel.DataAnnotations;
using System.Numerics;


namespace SocialNetwork.Dal.ViewModels.In
{
    public class ReactionCommentViewModel
    {
        [Required]
        public int CommentId { get; set; }
        public Guid ReactionId { get; set; }
    }
}
