using System;
using System.Numerics;

namespace SocialNetwork.Dal.Models
{
    public class ReactionComment
    {
        public Guid ReactionId { get; set; }
        public Guid UserId { get; set; }
        public BigInteger CommentId { get; set; }

        public virtual Comment Comment { get; set; }
        public virtual ReactionTypeComment Reaction { get; set; }
        public virtual User User { get; set; }
    }
}
