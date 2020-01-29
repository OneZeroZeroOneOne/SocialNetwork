using System;
using System.Collections.Generic;

namespace SocialNetwork.WebApi.Models
{
    public partial class Coments
    {
        public Coments()
        {
            PostComents = new HashSet<PostComents>();
        }

        public Guid Id { get; set; }
        public string Img { get; set; }

        public virtual ICollection<PostComents> PostComents { get; set; }
    }
}
