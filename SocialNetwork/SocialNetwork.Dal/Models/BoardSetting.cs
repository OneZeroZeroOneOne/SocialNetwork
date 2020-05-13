using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Dal.Models
{
    public class BoardSetting
    {
        public virtual Board Board { get; set; }
        public int SettingId { get; set; }
        public Guid BoardId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
