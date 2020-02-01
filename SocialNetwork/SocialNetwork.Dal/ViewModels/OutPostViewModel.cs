using System;

namespace SocialNetwork.Dal.ViewModels
{
    public class OutPostViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }

        // List<>
    }
}
