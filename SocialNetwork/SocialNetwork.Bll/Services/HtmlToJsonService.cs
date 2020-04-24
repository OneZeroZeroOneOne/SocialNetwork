using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SocialNetwork.Markdown.Jsonize;

namespace SocialNetwork.Bll.Services
{
    class HtmlToJsonService
    {
        public static string HtmlToJson(string inputHtml)
        {
            var json = new Jsonize(inputHtml).ParseHtmlToTypedJson().Children.FirstOrDefault();

            JsonSerializer jsonWriter = new JsonSerializer
            {
                NullValueHandling = (NullValueHandling)1
            };

            return JObject.FromObject(json, jsonWriter).ToString();
        }
    }
}
