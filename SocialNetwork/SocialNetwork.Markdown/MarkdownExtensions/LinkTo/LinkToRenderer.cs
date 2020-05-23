using Markdig.Renderers;
using Markdig.Renderers.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.RequestLifetimeBll.Services;
using SocialNetwork.Utilities.Exceptions;
using System;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Dapper;
using SocialNetwork.Dal.Models;

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

            if (requestLifetimeService == null) throw ExceptionFactory.SoftException(ExceptionEnum.SomethingWentWrong, $"If you see this message pls contact admin, {obj.Id}");

            var publicContext = (PublicContext)_serviceProvider.GetService(typeof(PublicContext));

            var isPostOrComment = publicContext.IsPostOrComment(obj.Id);

            var attr = new HtmlAttributes();

            attr.AddClass("link-to");

            attr.AddProperty("target", "_blank");
            attr.AddProperty("rel", "noopener noreferrer");

            attr.AddProperty("href", $"/{requestLifetimeService.Board.Name}/{requestLifetimeService.Post.Id}#{obj.Id}");
            attr.AddProperty("data-thread", requestLifetimeService.Post.Id.ToString());

            var innerText = ">>" + obj.Id;

            var mention = new Mention
            {
                MentionId = obj.Id,
                MentionerId = requestLifetimeService.MyId,
                IsComment = requestLifetimeService.IsComment,
            };

            requestLifetimeService.AddMention(mention);

            if (isPostOrComment == 0)
            {
                attr.AddProperty("data-post", obj.Id.ToString());
                innerText += " (OP)";
            }
            else if (isPostOrComment == 1)
                attr.AddProperty("data-comment", obj.Id.ToString());
            else
            {
                attr.AddProperty("data-post", obj.Id.ToString());
                attr.AddProperty("data-comment", obj.Id.ToString());
            }

            renderer.Write("<a").WriteAttributes(attr).Write($">{innerText}</a>");
        }
    }
}
