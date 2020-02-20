using System;
using System.Numerics;

namespace SocialNetwork.Dal.Models
{
    public class ReactionPost
    {
        public Guid ReactionId { get; set; }
        public Guid UserId { get; set; }
        public BigInteger PostId { get; set; }

        public virtual Post Post { get; set; }
        public virtual ReactionTypePost Reaction { get; set; }
        public virtual User User { get; set; }
    }
}
