using System;
using System.Collections.Generic;

namespace SocialNetwork.Dal.Models
{
    public partial class Comment
    {
        public Comment()
        {
            PostComment = new HashSet<PostComment>();
        }

        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string ImgUrl { get; set; }

        public virtual ICollection<PostComment> PostComment { get; set; }
    }
}
