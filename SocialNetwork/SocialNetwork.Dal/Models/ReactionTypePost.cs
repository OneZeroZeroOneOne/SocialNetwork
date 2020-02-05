using System;
using System.Collections.Generic;

namespace SocialNetwork.Dal.Models
{
    public partial class ReactionTypePost
    {
        public Guid ReactionId { get; set; }
        public string Content { get; set; }
    }
}
