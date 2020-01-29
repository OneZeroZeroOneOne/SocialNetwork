using System;
using System.Collections.Generic;

namespace SocialNetwork.WebApi.Models
{
    public partial class Users
    {
        public Users()
        {
            UserPosts = new HashSet<UserPosts>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserPosts> UserPosts { get; set; }
    }
}
