using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace SocialNetwork.Dal.ViewModels.In
{
    public class CommentViewModel
    {
        [Required]
        public string Text { get; set; }
        public List<int> ToPost { get; set; }
        public List<int> ToComment { get; set; }
    }
}
