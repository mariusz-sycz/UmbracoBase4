using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UmbracoBase4.App_Plugins.PushFiles
{
    public static class PushPromiseExtensions
    {
        public static IHtmlString PushPromiseStylesheet(this HtmlHelper htmlHelper, string contentPath)
        {
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            htmlHelper.ViewContext.RequestContext.HttpContext.Response.PushPromise(contentPath);

            TagBuilder linkTagBuilder = new TagBuilder("link");
            linkTagBuilder.Attributes.Add("rel", "stylesheet");
            linkTagBuilder.Attributes.Add("href", urlHelper.Content(contentPath));

            return new HtmlString(linkTagBuilder.ToString());
        }
        public static IHtmlString PushPromiseScript(this HtmlHelper htmlHelper, string contentPath)
        {
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            htmlHelper.ViewContext.RequestContext.HttpContext.Response.PushPromise(contentPath);

            TagBuilder linkTagBuilder = new TagBuilder("script");
            linkTagBuilder.Attributes.Add("src", urlHelper.Content(contentPath));

            return new HtmlString(linkTagBuilder.ToString());
        }
    }
}