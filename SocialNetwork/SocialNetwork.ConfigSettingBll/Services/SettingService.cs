using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.ConfigSetting.Dal.Context;
using SocialNetwork.ConfigSetting.Dal.Models;
using SocialNetwork.ConfigSettingBll.Abstractions;

namespace SocialNetwork.ConfigSetting.Bll.Services
{
    public class SettingService : ISettingService
    {
        private readonly ConfigSettingContext _configSettingContext;

        public SettingService(ConfigSettingContext configSettingContext)
        {
            _configSettingContext = configSettingContext;
        }

        public Task<Setting<T>> GetSetting<T>(string key)
        {
            return _configSettingContext.Set<Setting<T>>().FirstOrDefaultAsync(x => x.Key == key);
        }
    }
}
