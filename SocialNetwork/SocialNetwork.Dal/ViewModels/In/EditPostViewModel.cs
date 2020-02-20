using System;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace SocialNetwork.Dal.ViewModels.In
{
    public class EditPostViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
