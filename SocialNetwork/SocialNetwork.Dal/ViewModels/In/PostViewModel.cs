using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Dal.ViewModels.In
{
    public class PostViewModel
    {
        [Required]
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
