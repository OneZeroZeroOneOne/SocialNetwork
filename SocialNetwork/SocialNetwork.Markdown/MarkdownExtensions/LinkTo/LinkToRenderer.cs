using Markdig.Renderers;
using Markdig.Renderers.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.RequestLifetimeBll.Services;
using SocialNetwork.Utilities.Exceptions;
using System;

namespace SocialNetwork.Markdown.MarkdownExtensions.LinkTo
{
    public class LinkToRenderer : HtmlObjectRenderer<LinkToParsedModel>
    {
        private readonly IServiceProvider _serviceProvider;

        public LinkToRenderer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override void Write(HtmlRenderer renderer, LinkToParsedModel obj)
        {
            RequestLifetimeService requestLifetimeService = null;

            if (_serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext.Items
                .TryGetValue("RequestLifetime", out var lifetimeService))
            {
                requestLifetimeService = (RequestLifetimeService)lifetimeService;
            }

            //var comment = context.Comment.FirstOrDefault(x => x.Id == obj.Id);

            var attr = new HtmlAttributes();

            attr.AddClass("link-to");
            if (requestLifetimeService == null) throw ExceptionFactory.SoftException(ExceptionEnum.SomethingWentWrong, $"If you see this message pls contact admin, {obj.Id}"); ;

            attr.AddProperty("href",
                $"/{requestLifetimeService.Board.Name}/{requestLifetimeService.Post.Id}#{obj.Id}");
            attr.AddProperty("data-thread", requestLifetimeService.Post.Id.ToString());

            //if (comment != null)
            //{
            attr.AddProperty("data-id", obj.Id.ToString());
            //}

            var innerText = ">>" + obj.Id;
            if (requestLifetimeService.Post.Id == obj.Id)
                innerText += " (OP)";

            renderer.Write("<a").WriteAttributes(attr).Write($">{innerText}</a>");
        }
    }
}
