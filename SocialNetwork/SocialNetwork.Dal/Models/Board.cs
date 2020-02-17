using System;
using System.Collections.Generic;

namespace SocialNetwork.Dal.Models
{
    public partial class Board
    {
        public Board()
        {
            BoardPost = new HashSet<BoardPost>();
        }

        public Guid Id { get; set; }
        public Guid BoardTypeId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public bool IsArchived { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public virtual BoardType BoardType { get; set; }
        public virtual ICollection<BoardPost> BoardPost { get; set; }
    }
}
