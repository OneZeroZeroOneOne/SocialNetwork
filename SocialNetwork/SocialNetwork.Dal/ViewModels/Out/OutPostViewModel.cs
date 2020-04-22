using System;
using System.Collections.Generic;
using System.Numerics;
using SocialNetwork.Dal.Models;

namespace SocialNetwork.Dal.ViewModels.Out
{
    public class OutPostViewModel : Sortable
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public Guid UserId { get; set; }
        public List<OutAttachmentViewModel> AttachmentPost { get; set; }
    }
}
