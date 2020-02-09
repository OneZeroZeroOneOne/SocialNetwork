using System;
using System.Collections.Generic;

namespace SocialNetwork.Dal.Models
{
    public partial class Comment
    {
        public Comment()
        {
            ReactionComment = new HashSet<ReactionComment>();
        }
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public bool IsArchived { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<ReactionComment> ReactionComment { get; set; }
    }
}
