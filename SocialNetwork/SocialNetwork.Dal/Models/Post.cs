using System;
using System.Collections.Generic;

namespace SocialNetwork.Dal.Models
{
    public class Post : Sortable
    {
        public Post()
        {
            CommentPost = new HashSet<CommentPost>();
            ReactionPost = new HashSet<ReactionPost>();
            AttachmentPost = new HashSet<AttachmentPost>();
        }

        public int Id { get; set; }
        public string Text { get; set; }
        public Guid UserId { get; set; }
        public bool IsArchived { get; set; }

        public Guid BoardId { get; set; }

        public virtual Board Board { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<AttachmentPost> AttachmentPost { get; set; }
        public virtual ICollection<CommentPost> CommentPost { get; set; }
        public virtual ICollection<ReactionPost> ReactionPost { get; set; }
    }
}
