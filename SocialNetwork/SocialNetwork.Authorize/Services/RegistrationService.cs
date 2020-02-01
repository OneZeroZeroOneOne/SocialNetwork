using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Exceptions;
using SocialNetwork.Dal.Models;
using SocialNetwork.Security.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Security.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly PublicContext _publicContext;

        public RegistrationService(PublicContext publicContext)
        {
            _publicContext = publicContext;
        }

        public async Task<User> Register(string email, string password)
        {
            if (_publicContext.User.FirstOrDefault(x => x.Email == email) != null)
                throw ExceptionFactory.SoftException(ExceptionEnum.UserAlreadyExist,
                    $"User with {email} already exist");

            User userModel = new User()
            {
                Email = email,
                UserSecurity = new UserSecurity()
                {
                    Password = password,
                    Role = "Member",
                },
            };

            await _publicContext.User.AddAsync(userModel);
            await _publicContext.SaveChangesAsync();

            return userModel;
        }
    }
}
