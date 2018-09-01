using System;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Services;

namespace UmbracoExample
{
    public class PublishDateApplicationEventHandler : ApplicationEventHandler
    {
        protected override void ApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentService.Publishing += ContentService_Publishing;
        }

        private void ContentService_Publishing(Umbraco.Core.Publishing.IPublishingStrategy sender, Umbraco.Core.Events.PublishEventArgs<Umbraco.Core.Models.IContent> e)
        {
            var contentService = ApplicationContext.Current.Services.ContentService;
            foreach (var content in e.PublishedEntities.Where(m => m.HasProperty("publishDate")))
            {
                var existingValue = content.GetValue("publishDate");
                if (existingValue == null)
                {
                    content.SetValue("publishDate", DateTime.Now);
                }
            }
        }
    }
}