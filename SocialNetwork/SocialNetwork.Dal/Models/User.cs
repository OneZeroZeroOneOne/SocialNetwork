using System;
using System.Collections.Generic;

namespace SocialNetwork.Dal.Models
{
    public partial class User
    {
        public User()
        {
            UserPost = new HashSet<UserPost>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserPost> UserPost { get; set; }
    }
}
