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
            _configClient = new ConfigCatClient(new ManualPollConfiguration
            {
                ApiKey = apiKey
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
    }
}
