using System;
using System.Collections.Generic;

namespace SocialNetwork.Dal.Models
{
    public partial class ReactionTypePost
    {
        public ReactionTypePost()
        {
            ReactionPost = new HashSet<ReactionPost>();
        }

        public Guid ReactionId { get; set; }
        public string Content { get; set; }

        public virtual ICollection<ReactionPost> ReactionPost { get; set; }
    }
}
