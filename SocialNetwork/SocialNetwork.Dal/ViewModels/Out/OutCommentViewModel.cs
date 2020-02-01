using System;

namespace SocialNetwork.Dal.Models
{
    public partial class OutCommentViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public Guid PostId { get; set; }
    }
}
