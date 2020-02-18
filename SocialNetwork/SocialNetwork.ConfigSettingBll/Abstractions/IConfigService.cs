using SocialNetwork.Dal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialNetwork.ConfigSettingBll.Abstractions
{
    public interface IConfigService
    {
        public Task<T> GetSettingAsync<T>(string settingName, T defaultValue);

        public Task ForceRefreshAsync();

        public void ForceRefresh();

        public T GetSetting<T>(string settingName, T defaultValue);

        public Task<List<Setting<T>>> GetAllSettingsAsync<T>();
    }
}
