using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Utilities.Abstractions;
using System.Threading.Tasks;

namespace SocialNetwork.WebApi.Controllers
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
    }
}