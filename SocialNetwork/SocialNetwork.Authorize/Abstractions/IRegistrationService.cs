using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Dal.Models;

namespace SocialNetwork.Security.Abstractions
{
    public interface IRegistrationService
    {
        Task<User> Register(string email, string password);
    }
}
