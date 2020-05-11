using System.Collections.Generic;
using Ganss.XSS;
using Markdig;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.ConfigSettingBll.Abstractions;
using SocialNetwork.Markdown;
using SocialNetwork.Utilities.Exceptions;
using System.Linq;
using System.Threading.Tasks;
using Markdig.Parsers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Extensions;
using SocialNetwork.Markdown.Jsonize;
using SocialNetwork.Markdown.Jsonize.Models;

namespace SocialNetwork.Bll.Services
{
    public class UserInputService : IUserInputService
    {
        private readonly PublicContext _publicContext;
        private readonly ISettingService _settingService;
        private readonly IHtmlSanitizer _htmlSanitizer;

        private readonly MarkdownPipeline _pipeline;
        private readonly JsonSerializer _jsonWriter;

        public UserInputService(PublicContext publicContext, ISettingService settingService, IHtmlSanitizer htmlSanitizer)
        {
            _publicContext = publicContext;
            _settingService = settingService;
            _htmlSanitizer = htmlSanitizer;

            _htmlSanitizer.AllowedCssProperties.Clear();
            _htmlSanitizer.AllowedCssClasses.Clear();

            var pipeline = new MarkdownPipelineBuilder();

            var blockQuoteParser = pipeline.BlockParsers.Find<QuoteBlockParser>();
            if (blockQuoteParser != null) 
                pipeline.BlockParsers.Remove(blockQuoteParser);

            _pipeline = pipeline
                //.UseMediaLinks()
                .UseEmojiAndSmiley()
                .UseAutoLinks()
                .UseGreenText()
                .UseEmphasisExtras()
                .UseLinkTo()
                .UseMyEmphasis()
                .UseSoftlineBreakAsHardlineBreak()
                .DisableHtml()
                .Build();

            //pipeline.BlockParsers.Remove(QuoteBlockParser)
            //pipeline.BlockParsers.Tr();
            //pipeline.BlockParsers.TryRemove<HtmlBlockParser>();

            //pipeline.InlineParsers.TryRemove<HtmlEntityParser>();
            // pipeline.InlineParsers.TryRemove<CodeInlineParser>();
            //pipeline.InlineParsers.TryRemove<AutolineInlineParser>();


            _jsonWriter = new JsonSerializer
            {
                NullValueHandling = (NullValueHandling)1
            };
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

        private List<JsonizeNode> FlatNodes(JsonizeNode rootNode)
        {
            var stack = new List<JsonizeNode>();
            stack.AddRange(rootNode.Children);
            var result = new List<JsonizeNode>();

            while (stack.Count > 0)
            {
                var node = stack.PopAt(0);
                if (node.Children != null)
                {
                    stack.AddRange(node.Children);
                }

                result.Add(node);
            }

            return result;
        }

        public async Task<string> SanitizeHtml(string rawHtml)
        {
            await UpdateSettings();

            var sanitizedUserInput = _htmlSanitizer.Sanitize(rawHtml);
            sanitizedUserInput = sanitizedUserInput.Replace("&gt;", ">");

            return sanitizedUserInput;
        }

        private string ToMarkdown(string sanitizedUserInput)
        {
            return Markdig.Markdown.ToHtml(sanitizedUserInput, _pipeline);
        }

        public async Task<string> ParseStructure(string markdownText)
        {
            var nodes = new Jsonize(markdownText).ParseHtmlToTypedJson();//.Children.FirstOrDefault();//HtmlToJsonService.HtmlToJson(markdownText).Replace("\r\n", "");

            var flattenedNode = FlatNodes(nodes);

            foreach (var node in flattenedNode)
            {
                // ReSharper disable once StringLiteralTypo
                if (node.Tag == "linktocomponent")
                {
                    var (_, idValue) = node.Attributes.FirstOrDefault(x => x.Key == "id");
                    if (idValue != null)
                    {
                        if (!int.TryParse((string)idValue, out var intId))
                            throw ExceptionFactory.SoftException(ExceptionEnum.SomethingWentWrong,
                                "Something went wrong");

                        var linkToComment = await _publicContext.Comment.FirstOrDefaultAsync(x => x.Id == intId);
                        if (linkToComment == null) //first i check is it link to comment because link to comment have bigger chance to appear
                        {
                            var linkToPost = await _publicContext.Post.FirstOrDefaultAsync(x => x.Id == intId);
                            if (linkToPost != null)
                            {
                                node.Attributes.TryAdd("isPost", "true");
                                continue;
                            }

                            node.Attributes.TryAdd("isExist", "false");
                            continue;
                        }
                        node.Attributes.TryAdd("isComment", "true");
                    }
                }
            }

            return JObject.FromObject(nodes, _jsonWriter).ToString(); //.Replace("\r\n", "");
        }

        public async Task<string> Markdown(string userInput)
        {
            var sanitized = await SanitizeHtml(userInput);

            var markdown = ToMarkdown(sanitized);

            markdown = markdown.Replace("\n", "<br>");

            return await ParseStructure(markdown);
        }
    }
}
