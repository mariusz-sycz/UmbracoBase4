using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UmbracoBase4.App_Plugins.PushFiles
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class PushPromiseAttribute : FilterAttribute, IActionFilter
    {
        private PushPromiseTable _pushPromiseTable;

        public PushPromiseAttribute(PushPromiseTable pushPromiseTable)
        {
            if (pushPromiseTable == null)
            {
                throw new ArgumentNullException(nameof(pushPromiseTable));
            }

            _pushPromiseTable = pushPromiseTable;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        { }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException(nameof(filterContext));
            }

            IEnumerable<string> pushPromiseContentPaths = _pushPromiseTable.GetPushPromiseContentPaths();
            if (!filterContext.HttpContext.Request.Url.Query.Contains("amp=true"))
            {
                var linkHeader = string.Empty;
                foreach (string pushPromiseContentPath in pushPromiseContentPaths)
                {
                    var type = string.Empty;
                    filterContext.HttpContext.Response.PushPromise(pushPromiseContentPath);
                    if (pushPromiseContentPath.Contains(".png") || pushPromiseContentPath.Contains(".jpg") || pushPromiseContentPath.Contains(".svg"))
                    {
                        type = "image";
                    }
                    else if (pushPromiseContentPath.Contains(".js"))
                    {
                        type = "script";
                    }
                    else if (pushPromiseContentPath.Contains(".css"))
                    {
                        type = "style";
                    }
                    else if (pushPromiseContentPath.Contains(".woff") || pushPromiseContentPath.Contains(".woff2") || pushPromiseContentPath.Contains(".ttf"))
                    {
                        type = "font; crossorigin";
                    }
                    var link = string.Empty;
                    if(pushPromiseContentPath.Contains("https://") || pushPromiseContentPath.Contains("http://"))
                    {
                        link = pushPromiseContentPath;
                    }
                    else
                    {
                        link = System.Web.VirtualPathUtility.ToAbsolute(pushPromiseContentPath);
                    }
                    linkHeader += $"<{link}>; rel=preload; as={type},";

                }

                filterContext.HttpContext.Response.AddHeader("link", linkHeader);
            }
        }
    }
}