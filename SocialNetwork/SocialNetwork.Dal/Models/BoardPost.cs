using System;

namespace SocialNetwork.Dal.Models
{
    public partial class BoardPost
    {
        public Guid PostId { get; set; }
        public Guid BoardId { get; set; }

        public virtual Board Board { get; set; }
        public virtual Post Post { get; set; }
    }
}
