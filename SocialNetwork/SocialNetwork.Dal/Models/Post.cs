using System;
using System.Collections.Generic;

namespace SocialNetwork.Dal.Models
{
    public partial class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
            ReactionPost = new HashSet<ReactionPost>();
        }

        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }
        public bool IsArchived { get; set; }
        public virtual User User { get; set; }

        public virtual GroupPost GroupPost { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<ReactionPost> ReactionPost { get; set; }
    }
}
