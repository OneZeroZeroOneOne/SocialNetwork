using System;
using SocialNetwork.Dal.Models;
using System.Threading.Tasks;

namespace SocialNetwork.Security.Abstractions
{
    public interface IRegistrationService
    {
        Task<bool> ConfirmEmail(Guid confirmId);
        Task<User> Register(string email, string password);
    }
}
