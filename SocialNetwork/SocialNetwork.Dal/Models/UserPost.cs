using System;

namespace SocialNetwork.Dal.Models
{
    public partial class UserPost
    {
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
