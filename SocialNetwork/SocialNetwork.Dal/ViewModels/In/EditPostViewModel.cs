using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Dal.ViewModels.In
{
    public class EditPostViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
