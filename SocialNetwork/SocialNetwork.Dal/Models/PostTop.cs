using System;

namespace SocialNetwork.Dal.Models
{
    public class PostTop
    {
        public int PostId { get; set; }
        public Guid BoardId { get; set; }
        public int Score { get; set; }
    }
}
