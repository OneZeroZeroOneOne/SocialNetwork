using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Dal.ViewModels.In
{
    public class ReactionPostViewModel
    {
        [Required]
        public string PostId { get; set; }
        public string ReactionId { get; set; }
    }
}
