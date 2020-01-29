using System;
using System.Collections.Generic;

namespace SocialNetwork.WebApi.Models
{
    public partial class PostComents
    {
        public Guid PostId { get; set; }
        public Guid ComentId { get; set; }

        public virtual Coments Coment { get; set; }
        public virtual Posts Post { get; set; }
    }
}
