using System;
using System.Collections.Generic;
using System.Numerics;
using SocialNetwork.Dal.Models;

namespace SocialNetwork.Dal.ViewModels.Out
{
    public partial class OutCommentViewModel : Sortable
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int PostId { get; set; }

        public List<OutAttachmentViewModel> AttachmentComment { get; set; }
    }
}
