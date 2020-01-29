using System;
using System.Collections.Generic;

namespace SocialNetwork.WebApi.Models
{
    public partial class Posts
    {
        public Posts()
        {
            PostComents = new HashSet<PostComents>();
            UserPosts = new HashSet<UserPosts>();
        }

        public Guid Id { get; set; }
        public string Text { get; set; }
        public string Img { get; set; }

        public virtual ICollection<PostComents> PostComents { get; set; }
        public virtual ICollection<UserPosts> UserPosts { get; set; }
    }
}
