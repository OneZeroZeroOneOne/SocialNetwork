using Markdig;
using Markdig.Helpers;
using SocialNetwork.Markdown.MarkdownExtensions;

namespace SocialNetwork.Markdown
{
    public static class Markdown
    {
        public static MarkdownPipelineBuilder UseMyEmphasis(this MarkdownPipelineBuilder pipeline)
        {
            OrderedList<IMarkdownExtension> extensions;

            extensions = pipeline.Extensions;

            if (!extensions.Contains<MyEmphasisExtension>())
            {
                extensions.Add(new MyEmphasisExtension());
            }

            return pipeline;
        }

        public static MarkdownPipelineBuilder UseLinkTo(this MarkdownPipelineBuilder pipeline)
        {
            OrderedList<IMarkdownExtension> extensions;

            extensions = pipeline.Extensions;

            if (!extensions.Contains<LinkToExtension>())
            {
                extensions.Add(new LinkToExtension());
            }

            return pipeline;
        }
    }
}
