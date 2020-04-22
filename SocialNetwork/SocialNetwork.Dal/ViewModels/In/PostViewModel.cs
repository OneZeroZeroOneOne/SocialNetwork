using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace SocialNetwork.Dal.ViewModels.In
{
    public class PostViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public Guid BoardId { get; set; }
        public List<int> AttachmentIdList { get; set; }
    }
}
