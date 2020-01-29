using System;
using System.Collections.Generic;

namespace SocialNetwork.WebApi.Models
{
    public partial class UserPosts
    {
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }

        public virtual Posts Post { get; set; }
        public virtual Users User { get; set; }
    }
}
