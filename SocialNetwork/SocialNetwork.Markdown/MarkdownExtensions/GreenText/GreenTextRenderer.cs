using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace SocialNetwork.Markdown.MarkdownExtensions.GreenText
{
    class GreenTextRenderer : HtmlObjectRenderer<GreenTextParsedModel>
    {
        protected override void Write(HtmlRenderer renderer, GreenTextParsedModel obj)
        {
            var attr = new HtmlAttributes();

            attr.AddClass("green-text");

            renderer.Write("<span").WriteAttributes(attr).Write($">>{obj.Text}</span>");
        }
    }
}
