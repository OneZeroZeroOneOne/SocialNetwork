using SocialNetwork.Dal.Models;
using System;
using System.Collections.Generic;

namespace SocialNetwork.Dal.ViewModels
{
    public class OutPostViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }

        public List<OutCommentViewModel> Comments { get; set; }
    }
}
