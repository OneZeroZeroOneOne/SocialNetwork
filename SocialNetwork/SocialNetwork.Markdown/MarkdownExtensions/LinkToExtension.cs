using System;
using Markdig;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;
using SocialNetwork.Markdown.MarkdownExtensions.LinkTo;

namespace SocialNetwork.Markdown.MarkdownExtensions
{
    public class LinkToExtension : IMarkdownExtension
    {
        private readonly IServiceProvider _serviceProvider;

        public LinkToExtension(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            OrderedList<InlineParser> parsers;

            parsers = pipeline.InlineParsers;

            if (!parsers.Contains<LinkToParser>())
            {
                parsers.Add(new LinkToParser());
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            HtmlRenderer htmlRenderer;
            ObjectRendererCollection renderers;

            htmlRenderer = renderer as HtmlRenderer;
            renderers = htmlRenderer?.ObjectRenderers;

            if (renderers != null && !renderers.Contains<LinkToRenderer>())
            {
                renderers.Add(new LinkToRenderer(_serviceProvider));
            }
        }
    }
}
