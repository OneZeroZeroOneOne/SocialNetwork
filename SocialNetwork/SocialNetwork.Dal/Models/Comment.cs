using System;
using System.Collections.Generic;

namespace SocialNetwork.Dal.Models
{
    public class Comment : Sortable
    {
        public Comment()
        {
            AttachmentComment = new HashSet<AttachmentComment>();
            CommentComments = new HashSet<CommentComment>();
            ReplyToComment = new HashSet<CommentComment>();
            CommentPost = new HashSet<CommentPost>();
            ReactionComment = new HashSet<ReactionComment>();
        }

        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Text { get; set; }
        public bool IsArchived { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<AttachmentComment> AttachmentComment { get; set; }
        public virtual ICollection<CommentComment> CommentComments { get; set; }
        public virtual ICollection<CommentComment> ReplyToComment { get; set; }
        public virtual ICollection<CommentPost> CommentPost { get; set; }
        public virtual ICollection<ReactionComment> ReactionComment { get; set; }
    }
}
