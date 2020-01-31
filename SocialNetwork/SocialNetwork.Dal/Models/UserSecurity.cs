using System;

namespace SocialNetwork.Dal.Models
{
    public partial class UserSecurity
    {
        public Guid UserId { get; set; }

        public string Password { get; set; }
        public string Role { get; set; }
        public virtual User User { get; set; }
    }
}
