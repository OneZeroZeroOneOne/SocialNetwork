using System;
using System.Collections.Generic;

namespace SocialNetwork.Dal.Models
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            Posts = new HashSet<Post>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string AvatarUrl { get; set; }
        public string DateOfBirth { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
