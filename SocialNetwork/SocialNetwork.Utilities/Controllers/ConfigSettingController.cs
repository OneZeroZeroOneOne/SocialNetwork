using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Utilities.Abstractions;

namespace SocialNetwork.Utilities.Controllers
{
    [Route("[controller]")]
    public class ConfigSettingController : ControllerBase
    {
        private readonly IConfigSettingService _configSettingService;

        public ConfigSettingController(IConfigSettingService configSettingService)
        {
            _configSettingService = configSettingService;
        }

        [HttpPost]
        public async Task ConfigChanged()
        {
            await _configSettingService.ForceRefreshAsync();
        }

        [Authorize(Policy = "AdminUser")]
        [HttpGet]
        public async Task<string> GetSetting([FromQuery]string settingName)
        {
            return await _configSettingService.GetSettingAsync(settingName, "default value");
        }
    }
}