using System;

namespace SocialNetwork.Dal.Models
{
    public partial class ReactionTypeComment
    {
        public Guid ReactionId { get; set; }
        public string Content { get; set; }
    }
}
