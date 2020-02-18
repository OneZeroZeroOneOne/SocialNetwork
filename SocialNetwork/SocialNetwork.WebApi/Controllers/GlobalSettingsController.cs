using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialNetwork.ConfigSettingBll.Abstractions;
using SocialNetwork.Dal.Models;
using System.Threading.Tasks;

namespace SocialNetwork.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GlobalSettingsController : SocialNetworkBaseApiController
    {
        private readonly ILogger<GlobalSettingsController> _logger;
        private readonly ISettingService _settingService;

        public GlobalSettingsController(ILogger<GlobalSettingsController> logger, ISettingService settingService)
        {
            _logger = logger;
            _settingService = settingService;
        }

        [HttpGet, Route("n/{key}", Name = "numberSettingByKey")]
        public async Task<Setting<int>> GetNumberSetting([FromRoute]string key)
        {
            return await _settingService.GetSetting<int>(key);
        }

        [HttpGet, Route("s/{key}", Name = "stringSettingByKey")]
        public async Task<Setting<string>> GetStringSetting([FromRoute]string key)
        {
            return await _settingService.GetSetting<string>(key);
        }
    }
}