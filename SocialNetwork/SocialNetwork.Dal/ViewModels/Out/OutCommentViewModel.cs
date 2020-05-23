using SocialNetwork.Dal.Models;
using System.Collections.Generic;

namespace SocialNetwork.Dal.ViewModels.Out
{
    public partial class OutCommentViewModel : Sortable
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int PostId { get; set; }
        public int SeqId { get; set; }

        public List<OutAttachmentViewModel> AttachmentComment { get; set; }
    }
}
