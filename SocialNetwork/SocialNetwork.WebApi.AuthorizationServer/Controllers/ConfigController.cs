using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Utilities.Abstractions;

namespace SocialNetwork.WebApi.AuthorizationServer.Controllers
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