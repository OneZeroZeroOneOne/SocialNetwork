using System.Threading.Tasks;
using SocialNetwork.Dal.Models;

namespace SocialNetwork.ConfigSettingBll.Abstractions
{
    public interface ISettingService
    {
        public Task<Setting<T>> GetSetting<T>(string key);
    }
}
