using System;

namespace SocialNetwork.Dal.Models
{
    public partial class GroupPost
    {
        public Guid PostId { get; set; }
        public Guid GroupId { get; set; }

        public virtual Group Group { get; set; }
        public virtual Post Post { get; set; }
    }
}
