using ConfigCat.Client;
using SocialNetwork.ConfigSettingBll.Abstractions;
using SocialNetwork.Dal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialNetwork.ConfigSettingBll.Services
{
    public class ConfigService : IConfigService
    {
        private readonly ConfigCatClient _configClient;

        public ConfigService(string apiKey)
        {
            _configClient = new ConfigCatClient(new LazyLoadConfiguration
            {
                ApiKey = apiKey, // <-- This is the actual API key for your Production environment
                CacheTimeToLiveSeconds = 720 * 60,
            });
        }

        public async Task<T> GetSettingAsync<T>(string settingName, T defaultValue)
        {
            return await _configClient.GetValueAsync(settingName, defaultValue);
        }

        public async Task ForceRefreshAsync()
        { 
            await _configClient.ForceRefreshAsync();
        }

        public void ForceRefresh()
        {
            _configClient.ForceRefresh();
        }

        public T GetSetting<T>(string settingName, T defaultValue)
        {
            return _configClient.GetValue(settingName, defaultValue);
        }

        public async Task<List<Setting<T>>> GetAllSettingsAsync<T>()
        {
            var keys = await _configClient.GetAllKeysAsync();

            var settings = new List<Setting<T>>();

            foreach (var key in keys)
            {
                settings.Add(new Setting<T>()
                {
                    Key = key,
                    Value = await _configClient.GetValueAsync<T>(key, default)
                });
            }

            return settings;
        }
    }
}
