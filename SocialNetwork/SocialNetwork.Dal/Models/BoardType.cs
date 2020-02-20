using System;
using System.Collections.Generic;

namespace SocialNetwork.Dal.Models
{
    public class BoardType
    {
        public BoardType()
        {
            Board = new HashSet<Board>();
        }

        public Guid Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Board> Board { get; set; }
    }
}
