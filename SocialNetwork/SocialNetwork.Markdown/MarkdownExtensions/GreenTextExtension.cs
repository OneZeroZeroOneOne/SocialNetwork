using Markdig;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;
using SocialNetwork.Markdown.MarkdownExtensions.GreenText;

namespace SocialNetwork.Markdown.MarkdownExtensions
{
    class GreenTextExtension : IMarkdownExtension
    {
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            OrderedList<InlineParser> parsers;

            parsers = pipeline.InlineParsers;

            if (!parsers.Contains<GreenTextParser>())
            {
                parsers.Add(new GreenTextParser());
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            HtmlRenderer htmlRenderer;
            ObjectRendererCollection renderers;

            htmlRenderer = renderer as HtmlRenderer;
            renderers = htmlRenderer?.ObjectRenderers;

            if (renderers != null && !renderers.Contains<GreenTextRenderer>())
            {
                renderers.Add(new GreenTextRenderer());
            }
        }
    }
}
