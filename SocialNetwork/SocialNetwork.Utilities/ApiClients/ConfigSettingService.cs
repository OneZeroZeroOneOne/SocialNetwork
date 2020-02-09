using System.Collections.Generic;
using ConfigCat.Client;
using SocialNetwork.Utilities.Abstractions;
using System.Threading.Tasks;

namespace SocialNetwork.Utilities.ApiClients
{
    public class ConfigSettingService : IConfigSettingService
    {
        private readonly ConfigCatClient _configClient;

        public ConfigSettingService(string apiKey)
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

        public async Task<List<Setting>> GetAllSettingsAsync()
        {
            var keys = await _configClient.GetAllKeysAsync();

            var settings = new List<Setting>();

            foreach (var key in keys)
            {
                settings.Add(new Setting()
                {
                    Key = key,
                    Value = await _configClient.GetValueAsync(key, "non present")
                });
            }

            return settings;
        }
    }
}
