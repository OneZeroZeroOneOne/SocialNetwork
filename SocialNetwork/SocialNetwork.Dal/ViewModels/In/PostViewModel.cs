using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace SocialNetwork.Dal.ViewModels.In
{
    public class PostViewModel
    {
        [Required]
        public string Text { get; set; }
        [Required]
        public Guid BoardId { get; set; }
        public List<int> AttachmentIdList { get; set; }
    }
}
