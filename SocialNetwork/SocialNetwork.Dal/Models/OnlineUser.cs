using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Dal.Models
{
    public partial class OnlineUser 
    {
        public Guid UserId { get; set; }

        public string Token { get; set; }


    }
}
