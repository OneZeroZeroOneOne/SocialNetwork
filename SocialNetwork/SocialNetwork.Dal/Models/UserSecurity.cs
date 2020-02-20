using System;
using System.Collections.Generic;

namespace SocialNetwork.Dal.Models
{
    public class UserSecurity
    {
        public UserSecurity()
        {
            UserConfirmationTokens = new HashSet<UserConfirmationToken>();
        }

        public Guid UserId { get; set; }

        public string Password { get; set; }

        public virtual Role Role { get; set; }

        public bool IsConfirmed { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<UserConfirmationToken> UserConfirmationTokens { get; set; }
    }
}
