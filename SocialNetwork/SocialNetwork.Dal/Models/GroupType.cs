using System;
using System.Collections.Generic;

namespace SocialNetwork.Dal.Models
{
    public partial class GroupType
    {
        public GroupType()
        {
            Group = new HashSet<Group>();
        }

        public Guid Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Group> Group { get; set; }
    }
}
