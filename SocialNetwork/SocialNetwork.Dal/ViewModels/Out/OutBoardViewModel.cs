using System;
using System.Collections.Generic;

namespace SocialNetwork.Dal.ViewModels.Out
{
    public partial class OutBoardViewModel
    {
        public Guid Id { get; set; }
        public Guid BoardTypeId { get; set; }
        public bool IsArchived { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}