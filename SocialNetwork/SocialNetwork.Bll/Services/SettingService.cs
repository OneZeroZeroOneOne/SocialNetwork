using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Models;

namespace SocialNetwork.Bll.Services
{
    public class SettingService : ISettingService
    {
        private readonly PublicContext _publicContext;

        public SettingService(PublicContext publicContext)
        {
            _publicContext = publicContext;
        }

        public Task<Setting<T>> GetSetting<T>(string key)
        {
            return _publicContext.Set<Setting<T>>().FirstOrDefaultAsync(x => x.Key == key);
        }
    }
}
