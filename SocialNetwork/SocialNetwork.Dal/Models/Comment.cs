﻿using System;
using System.Collections.Generic;
using System.Numerics;

namespace SocialNetwork.Dal.Models
{
    public class Comment : Sortable
    {
        public Comment()
        {
            ReactionComment = new HashSet<ReactionComment>();
            AttachmentComment = new HashSet<AttachmentComment>();
        }
        public BigInteger Id { get; set; }
        public string Text { get; set; }
        public Guid UserId { get; set; }
        public BigInteger PostId { get; set; }
        public bool IsArchived { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<AttachmentComment> AttachmentComment { get; set; }
        public virtual ICollection<ReactionComment> ReactionComment { get; set; }
    }
}
