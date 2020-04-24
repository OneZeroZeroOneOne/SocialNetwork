using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace SocialNetwork.Markdown.MarkdownExtensions.LinkTo
{
    public class LinkToRenderer : HtmlObjectRenderer<LinkToParsedModel>
    {
        protected override void Write(HtmlRenderer renderer, LinkToParsedModel obj)
        {
            var attr = new HtmlAttributes();
            attr.AddProperty("id", obj.Id.ToString());//.SetData();

            renderer.Write("<LinkToComponent").WriteAttributes(attr).Write("></LinkToComponent>");
        }
    }
}
