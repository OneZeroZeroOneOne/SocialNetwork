using System;
using System.Collections.Generic;

namespace SocialNetwork.Dal.Models
{
    public partial class ReactionTypeComment
    {
        public ReactionTypeComment()
        {
            ReactionComment = new HashSet<ReactionComment>();
        }

        public Guid ReactionId { get; set; }
        public string Content { get; set; }

        public virtual ICollection<ReactionComment> ReactionComment { get; set; }
    }
}
