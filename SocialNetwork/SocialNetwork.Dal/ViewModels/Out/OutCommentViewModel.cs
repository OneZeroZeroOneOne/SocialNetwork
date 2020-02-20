using System;
using System.Numerics;

namespace SocialNetwork.Dal.ViewModels.Out
{
    public partial class OutCommentViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int PostId { get; set; }
    }
}
