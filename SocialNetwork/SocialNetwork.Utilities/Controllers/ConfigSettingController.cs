using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.ConfigSettingBll.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;
using SocialNetwork.Dal.Models;

namespace SocialNetwork.Utilities.Controllers
{
    [Route("[controller]")]
    public class ConfigSettingController : ControllerBase
    {
        private readonly IConfigService _configSettingService;

        public ConfigSettingController(IConfigService configSettingService)
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

        //[Authorize(Policy = "AdminUser")]
        [HttpGet("All")]
        public async Task<List<Setting<string>>> GetSettingAll()
        {
            return await _configSettingService.GetAllSettingsAsync<string>();
        }
    }
}