using System.Numerics;

namespace SocialNetwork.Dal.Models
{
    public class AttachmentComment
    {
        public BigInteger CommentId { get; set; }
        public BigInteger AttachmentId { get; set; }

        public virtual Attachment Attachment { get; set; }
        public virtual Comment Comment { get; set; }
    }
}
