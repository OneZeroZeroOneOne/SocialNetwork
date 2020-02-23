using System.Numerics;

namespace SocialNetwork.Dal.Models
{
    public class AttachmentComment
    {
        public int CommentId { get; set; }
        public int AttachmentId { get; set; }

        public virtual Attachment Attachment { get; set; }
    }
}
