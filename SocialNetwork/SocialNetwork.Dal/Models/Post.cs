﻿using System;
using System.Collections.Generic;

namespace SocialNetwork.Dal.Models 
{
    public partial class Post : Sortable
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
            ReactionPost = new HashSet<ReactionPost>();
            AttachmentPost = new HashSet<AttachmentPost>();
            BoardPost = new HashSet<BoardPost>();
        }

        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid UserId { get; set; }
        public bool IsArchived { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<BoardPost> BoardPost { get; set; }
        public virtual ICollection<AttachmentPost> AttachmentPost { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<ReactionPost> ReactionPost { get; set; }
    }
}
