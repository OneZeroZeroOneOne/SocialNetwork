using System;

namespace SocialNetwork.Dal.Models
{
    public partial class AttachmentPost
    {
        public Guid PostId { get; set; }
        public Guid AttachmentId { get; set; }

        public virtual Attachment Attachment { get; set; }
        public virtual Post Post { get; set; }
    }
}
