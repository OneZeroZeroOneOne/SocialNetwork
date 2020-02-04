using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Dal.Models
{
    public partial class CommentReaction
    {
        public Guid Id { get; set; }
        public Guid CommentId { get; set; }

        public Guid UserId { get; set; }

        public DateTime? Date { get; set; }

        public int ReactionId { get; set; }


    }
}
