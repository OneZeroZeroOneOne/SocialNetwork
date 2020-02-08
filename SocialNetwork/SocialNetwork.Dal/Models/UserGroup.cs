using System;

namespace SocialNetwork.Dal.Models
{
    public partial class UserGroup
    {
        public Guid GroupId { get; set; }
        public Guid UserId { get; set; }

        public virtual Group Group { get; set; }
        public virtual User User { get; set; }
    }
}
