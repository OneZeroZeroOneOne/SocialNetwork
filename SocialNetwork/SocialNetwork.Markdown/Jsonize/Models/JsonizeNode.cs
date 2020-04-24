using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;

namespace SocialNetwork.Markdown.Jsonize.Models
{
    public class JsonizeNode
    {
        [JsonProperty(PropertyName = "node_id")]
        public int NodeId { get; set; }

        [JsonProperty(PropertyName = "parent_id")]
        public int ParentId { get; set; }

        [JsonProperty(PropertyName = "node")]
        public string Node { get; set; }
        
        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "attr")]
        public ExpandoObject Attributes { get; set; }

        [JsonProperty(PropertyName = "child")]
        public List<JsonizeNode> Children { get; set; }
    }
}
