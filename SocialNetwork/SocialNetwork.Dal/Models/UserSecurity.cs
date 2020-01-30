using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Dal.Models
{
    public partial class UserSecurity
    {
        public Guid UserId { get; set; }

        public string Password { get; set; }

        public string Mail { get; set; }

        public virtual User User { get; set; }


    }
}
