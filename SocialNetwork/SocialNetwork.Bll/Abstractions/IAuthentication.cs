using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Bll.Abstractions
{
    public interface IAuthentication
    {

        ClaimsIdentity GetIdentity(string username, string password);
    }
}
