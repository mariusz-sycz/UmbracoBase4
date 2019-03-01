using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UmbracoBase4.App_Plugins.PushFiles
{
    public class UmbracoApplicationWithHttpPush: Umbraco.Web.UmbracoApplication
    {
        protected override void OnApplicationStarting(object sender, EventArgs e)
        {
            base.OnApplicationStarting(sender, e);
            PushPromiseTable pushPromiseTable = new PushPromiseTable();
            pushPromiseTable.MapPushPromise("*", "*", "~/css/min.css");
            pushPromiseTable.MapPushPromise("*", "*", "~/js/app.min.js");
            pushPromiseTable.MapPushPromise("*", "*", "~/fonts/7XUFZ5tgS-tD6QamInJTcSo_WB_cotcEMUw1LsIE8mM.woff2");
            pushPromiseTable.MapPushPromise("*", "*", "~/fonts/7XUFZ5tgS-tD6QamInJTcZSnX671uNZIV63UdXh3Mg0.woff2");
            pushPromiseTable.MapPushPromise("*", "*", "~/images/amp/fb_icon.png");
            //pushPromiseTable.MapPushPromise("*", "*", "https://ssl.gstatic.com/images/icons/gplus-32.png");
            pushPromiseTable.MapPushPromise("*", "*", "~/Images/amp/t_icon.png");
            pushPromiseTable.MapPushPromise("*", "*", "~/Images/amp/linkedin.png");
            pushPromiseTable.MapPushPromise("*", "*", "~/images/AMP/close.svg");
            pushPromiseTable.MapPushPromise("*", "*", "~/images/AMP/return.svg");
            pushPromiseTable.MapPushPromise("*", "*", "~/images/AMP/sprite.svg");
            pushPromiseTable.MapPushPromise("*", "*", "~/images/AMP/search.svg");
            pushPromiseTable.MapPushPromise("*", "*", "~/images/AMP/hamburger.svg");
            pushPromiseTable.MapPushPromise("*", "*", "~/favicon.ico");
            pushPromiseTable.MapPushPromise("*", "*", "~/service-worker.js");
            pushPromiseTable.MapPushPromise("*", "*", "~/cdn-cgi/scripts/5c5dd728/cloudflare-static/email-decode.min.js");
            //pushPromiseTable.MapPushPromise("Frame", "FrameByToken", "~/fonts/pictos/PictosComplete-Regular.woff");
            //pushPromiseTable.MapPushPromise("Frame", "FrameByToken", "~/fonts/renault-life-latin/RenaultLifeWeb-Regular.woff");

            GlobalFilters.Filters.Add(new PushPromiseAttribute(pushPromiseTable));
        }
    }
}