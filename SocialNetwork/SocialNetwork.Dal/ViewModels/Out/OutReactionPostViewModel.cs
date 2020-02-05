using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Dal.ViewModels.Out
{
    class OutReactionPostViewModel
    {
        public Guid ReactionId { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }

    }
}
