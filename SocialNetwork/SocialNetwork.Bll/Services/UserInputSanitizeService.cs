using Ganss.XSS;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.ConfigSettingBll.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Bll.Services
{
    public class UserInputSanitizeService : IUserInputSanitizeService
    {
        private readonly ISettingService _settingService;
        private readonly IHtmlSanitizer _htmlSanitizer;

        public UserInputSanitizeService(ISettingService settingService, IHtmlSanitizer htmlSanitizer)
        {
            _settingService = settingService;
            _htmlSanitizer = htmlSanitizer;

            _htmlSanitizer.AllowedCssProperties.Clear();
            _htmlSanitizer.AllowedCssClasses.Clear();
        }

        private async Task UpdateSettings()
        {
            _htmlSanitizer.AllowedTags.Clear();

            _htmlSanitizer.AllowedTags.UnionWith((await _settingService.GetSetting<string>("AllowedHtmlTags")).Value.Split(',').AsEnumerable());
        }

        public async Task<string> SanitizeHtml(string rawHtml)
        {
            await UpdateSettings();

            return _htmlSanitizer.Sanitize(rawHtml);
        }
    }
}
