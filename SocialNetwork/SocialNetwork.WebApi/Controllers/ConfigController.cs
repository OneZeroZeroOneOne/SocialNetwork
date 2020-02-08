using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Utilities.Abstractions;
using SocialNetwork.Utilities.Controllers;

namespace SocialNetwork.WebApi.Controllers
{
    [Route("[controller]")]
    public class ConfigController : ConfigSettingController
    {
        public ConfigController(IConfigSettingService configSettingService)
            : base(configSettingService)
        {

        }
    }
}