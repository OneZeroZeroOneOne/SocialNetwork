using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Utilities.Abstractions
{
    public interface IMailClient
    {
        Task<bool> SendConfirmationMail(string email, string confirmUrl);
    }
}
