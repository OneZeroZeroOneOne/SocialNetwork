using System.Collections.Generic;
using System.Threading.Tasks;
using SocialNetwork.ConfigSetting.Dal.Models;

namespace SocialNetwork.ConfigSetting.Bll.Abstractions
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
