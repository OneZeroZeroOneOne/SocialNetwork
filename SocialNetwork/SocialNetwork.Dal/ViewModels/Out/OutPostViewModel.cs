using SocialNetwork.Dal.Models;
using System;
using System.Collections.Generic;

namespace SocialNetwork.Dal.ViewModels.Out
{
    public class OutPostViewModel : Sortable
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public Guid UserId { get; set; }
        public Guid BoardId { get; set; }
        public List<OutAttachmentViewModel> AttachmentPost { get; set; }
        public List<OutMentionViewModel> Mention { get; set; }
    }
}
