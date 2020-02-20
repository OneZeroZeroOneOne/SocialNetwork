using System;
using System.Numerics;

namespace SocialNetwork.Dal.ViewModels.Out
{
    public partial class OutCommentViewModel
    {
        public BigInteger Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public BigInteger PostId { get; set; }
    }
}
