using System;
using System.Collections.Generic;

namespace SocialNetwork.Dal.Models
{
    public partial class Attachment
    {
        public Attachment()
        {
            AttachmentComment = new HashSet<AttachmentComment>();
            AttachmentPost = new HashSet<AttachmentPost>();
        }

        public Guid Id { get; set; }
        public string ContentType { get; set; }
        public string Path { get; set; }
        public DateTime CreateDateTime { get; set; }

        public virtual ICollection<AttachmentComment> AttachmentComment { get; set; }
        public virtual ICollection<AttachmentPost> AttachmentPost { get; set; }
    }
}
