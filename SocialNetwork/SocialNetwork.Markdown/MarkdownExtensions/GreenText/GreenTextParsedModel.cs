using Markdig.Syntax.Inlines;

namespace SocialNetwork.Markdown.MarkdownExtensions.GreenText
{
    class GreenTextParsedModel : LeafInline
    {
        public string Text { get; set; }
    }
}
