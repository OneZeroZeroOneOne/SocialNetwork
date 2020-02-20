using System.Numerics;

namespace SocialNetwork.Dal.Models
{
    public class AttachmentPost
    {
        public BigInteger PostId { get; set; }
        public BigInteger AttachmentId { get; set; }

        public virtual Attachment Attachment { get; set; }
        public virtual Post Post { get; set; }
    }
}
