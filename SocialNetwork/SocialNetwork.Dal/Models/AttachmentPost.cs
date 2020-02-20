using System.Numerics;

namespace SocialNetwork.Dal.Models
{
    public class AttachmentPost
    {
        public int PostId { get; set; }
        public int AttachmentId { get; set; }

        public virtual Attachment Attachment { get; set; }
        public virtual Post Post { get; set; }
    }
}
