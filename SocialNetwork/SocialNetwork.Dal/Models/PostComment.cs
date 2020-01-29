using System;

namespace SocialNetwork.Dal.Models
{
    public partial class PostComment
    {
        public Guid PostId { get; set; }
        public Guid CommentId { get; set; }

        public virtual Comment Comment { get; set; }
        public virtual Post Post { get; set; }
    }
}
