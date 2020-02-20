using System;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace SocialNetwork.Dal.ViewModels.In
{
    public class ReactionPostViewModel
    {
        [Required]
        public int PostId { get; set; }
        public Guid ReactionId { get; set; }
    }
}
