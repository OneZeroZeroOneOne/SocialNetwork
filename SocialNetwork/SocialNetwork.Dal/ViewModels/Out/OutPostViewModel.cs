using System;
using System.Collections.Generic;
using System.Numerics;

namespace SocialNetwork.Dal.ViewModels.Out
{
    public class OutPostViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }
        public List<OutAttachmentViewModel> AttachmentPost { get; set; }
    }
}
