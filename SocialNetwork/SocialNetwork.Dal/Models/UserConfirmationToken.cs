using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Dal.Models
{
    public class UserConfirmationToken
    {
        public Guid UserId { get; set; }

        public string ConfirmEmail { get; set; }
        public DateTime CreateDateTime { get; set; }
        public Guid ConfirmId { get; set; }

        public bool IsActive { get; set; }

        public virtual UserSecurity UserSecurity { get; set; }
        
    }
}
