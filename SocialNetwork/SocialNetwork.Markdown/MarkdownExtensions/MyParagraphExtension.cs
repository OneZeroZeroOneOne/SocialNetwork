using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace SocialNetwork.Markdown.MarkdownExtensions
{
    public class MyParagraphExtension : IMarkdownExtension
    {
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            // Do I need to implement this?
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            renderer.ObjectRenderers.RemoveAll(x => x is ParagraphRenderer);
            renderer.ObjectRenderers.Add(new MyParagraphRenderer());
        }
    }

    public class MyParagraphRenderer : ParagraphRenderer
    {
        protected override void Write(HtmlRenderer renderer, ParagraphBlock obj)
        {
            if (!renderer.IsFirstInContainer)
            {
                renderer.EnsureLine();
            }
            renderer.WriteLeafInline(obj);
            if (!renderer.IsLastInContainer)
            {
                renderer.WriteLine("<br />");
                renderer.WriteLine("<br />");
            }
        }
    }
}
