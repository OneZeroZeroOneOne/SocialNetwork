using System;
using System.Collections.Generic;

namespace SocialNetwork.Dal.Models
{
    public partial class Post
    {
        public Post()
        {
            PostComment = new HashSet<PostComment>();
            UserPost = new HashSet<UserPost>();
        }

        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string ImgUrl { get; set; }

        public virtual ICollection<PostComment> PostComment { get; set; }
        public virtual ICollection<UserPost> UserPost { get; set; }
    }
}
