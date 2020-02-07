using System.Threading.Tasks;
using ConfigCat.Client;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Utilities.Abstractions;

namespace SocialNetwork.WebApi.Controllers
{
    [Route("[controller]")]
    public class ConfigSettingController : ControllerBase
    {
        private readonly IConfigSettingService _configCatClient;

        public ConfigSettingController(IConfigSettingService configCatClient) // Inject the ConfigCat client.
        {
            _configCatClient = configCatClient;
        }

        [HttpPost]
        public async Task ConfigCatChanged()
        {
            await _configCatClient.ForceRefreshAsync(); // Manually refresh the cache with the new setting values.
        }
    }
}