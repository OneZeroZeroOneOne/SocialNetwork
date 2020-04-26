using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace SocialNetwork.Markdown.MarkdownExtensions.GreenText
{
    class GreenTextRenderer : HtmlObjectRenderer<GreenTextParsedModel>
    {
        protected override void Write(HtmlRenderer renderer, GreenTextParsedModel obj)
        {
            renderer.Write("<GreenComponent>").Write(obj.Text).Write("</GreenComponent>");
        }
    }
}
