using SocialNetwork.ConfigSetting.Dal.Models;
using System.Threading.Tasks;

namespace SocialNetwork.ConfigSettingBll.Abstractions
{
    public interface ISettingService
    {
        public Task<Setting<T>> GetSetting<T>(string key);
    }
}
