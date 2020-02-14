using System;

namespace SocialNetwork.Dal.Models
{
    public partial class AttachmentComment
    {
        public Guid CommentId { get; set; }
        public Guid AttachmentId { get; set; }

        public virtual Attachment Attachment { get; set; }
        public virtual Comment Comment { get; set; }
    }
}
