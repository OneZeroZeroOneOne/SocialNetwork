using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SocialNetwork.Dal.ViewModels
{
    public class EditPostViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
