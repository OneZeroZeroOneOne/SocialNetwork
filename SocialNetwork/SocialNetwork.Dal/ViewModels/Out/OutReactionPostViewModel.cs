using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SocialNetwork.Dal.ViewModels.Out
{
    public class OutReactionPostViewModel
    {
        public Guid ReactionId { get; set; }
        public Guid UserId { get; set; }
        public int PostId { get; set; }

    }
}
