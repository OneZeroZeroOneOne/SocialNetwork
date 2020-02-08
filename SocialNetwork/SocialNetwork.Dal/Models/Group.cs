using System;
using System.Collections.Generic;

namespace SocialNetwork.Dal.Models
{
    public partial class Group
    {
        public Group()
        {
            GroupPost = new HashSet<GroupPost>();
            UserGroup = new HashSet<UserGroup>();
        }

        public Guid Id { get; set; }
        public Guid GroupTypeId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public bool IsArchived { get; set; }

        public virtual GroupType GroupType { get; set; }
        public virtual ICollection<GroupPost> GroupPost { get; set; }
        public virtual ICollection<UserGroup> UserGroup { get; set; }
    }
}
