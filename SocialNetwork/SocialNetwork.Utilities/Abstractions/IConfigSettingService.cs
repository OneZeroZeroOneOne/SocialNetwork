using System.Threading.Tasks;

namespace SocialNetwork.Utilities.Abstractions
{
    public interface IConfigSettingService
    {
        public Task<T> GetSettingAsync<T>(string settingName, T defaultValue);

        public Task ForceRefreshAsync();

        public void ForceRefresh();

        public T GetSetting<T>(string settingName, T defaultValue);
    }
}
