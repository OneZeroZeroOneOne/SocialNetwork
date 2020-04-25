using Ganss.XSS;
using Markdig;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.ConfigSettingBll.Abstractions;
using SocialNetwork.Markdown;
using SocialNetwork.Utilities.Exceptions;
using System.Linq;
using System.Threading.Tasks;
using Markdig.Parsers;

namespace SocialNetwork.Bll.Services
{
    public class UserInputService : IUserInputService
    {
        private readonly ISettingService _settingService;
        private readonly IHtmlSanitizer _htmlSanitizer;

        private readonly MarkdownPipeline _pipeline;

        public UserInputService(ISettingService settingService, IHtmlSanitizer htmlSanitizer)
        {
            _settingService = settingService;
            _htmlSanitizer = htmlSanitizer;

            _htmlSanitizer.AllowedCssProperties.Clear();
            _htmlSanitizer.AllowedCssClasses.Clear();

            var pipeline = new MarkdownPipelineBuilder();

            var blockQuoteParser = pipeline.BlockParsers.Find<QuoteBlockParser>();
            if (blockQuoteParser != null) 
                pipeline.BlockParsers.Remove(blockQuoteParser);

            _pipeline = pipeline
                .UseEmphasisExtras()
                .UseLinkTo()
                .UseMyEmphasis()
                .Build();

            //pipeline.BlockParsers.Remove(QuoteBlockParser)
            //pipeline.BlockParsers.Tr();
            //pipeline.BlockParsers.TryRemove<HtmlBlockParser>();

            //pipeline.InlineParsers.TryRemove<HtmlEntityParser>();
            // pipeline.InlineParsers.TryRemove<CodeInlineParser>();
            //pipeline.InlineParsers.TryRemove<AutolineInlineParser>();
        }

        private async Task UpdateSettings()
        {
            _htmlSanitizer.AllowedTags.Clear();
            _htmlSanitizer.AllowedAttributes.Clear();

            var allowedHtmlTags = await _settingService.GetSetting<string>("AllowedHtmlTags");
            var allowedAttributes = await _settingService.GetSetting<string>("AllowedHtmlAttributes");


            if (allowedHtmlTags?.Value == null)
                throw ExceptionFactory.UserFriendlyException(ExceptionEnum.GlobalSettingNotFound, "global setting allowed html tags doesn't found");


            if (allowedAttributes?.Value == null)
                throw ExceptionFactory.UserFriendlyException(ExceptionEnum.GlobalSettingNotFound, "global setting allowed attributes doesn't found");

            _htmlSanitizer.AllowedTags.UnionWith(allowedHtmlTags.Value.Split(',').AsEnumerable());
            _htmlSanitizer.AllowedAttributes.UnionWith(allowedAttributes.Value.Split(',').AsEnumerable());
        }

        public async Task<string> SanitizeHtml(string rawHtml)
        {
            await UpdateSettings();

            var sanitizedUserInput = _htmlSanitizer.Sanitize(rawHtml);
            sanitizedUserInput = sanitizedUserInput.Replace("&gt;", ">");

            var result = Markdig.Markdown.ToHtml(sanitizedUserInput, _pipeline);

            return HtmlToJsonService.HtmlToJson(result).Replace("\r\n","");
        }
    }
}
