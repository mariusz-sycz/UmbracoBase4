using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UmbracoBase4.App_Plugins.PushFiles
{
    public class PushPromiseTable
    {
        private readonly IDictionary<string, IDictionary<string, ICollection<string>>> _pushPromiseTable =
            new Dictionary<string, IDictionary<string, ICollection<string>>>();

        public void MapPushPromise(string controller, string action, string contentPath)
        {
            if (!_pushPromiseTable.ContainsKey(controller))
            {
                _pushPromiseTable.Add(controller, new Dictionary<string, ICollection<string>>());
            }

            if (!_pushPromiseTable[controller].ContainsKey(action))
            {
                _pushPromiseTable[controller].Add(action, new List<string>());
            }

            _pushPromiseTable[controller][action].Add(contentPath);
        }

        internal IEnumerable<string> GetPushPromiseContentPaths(string controller, string action)
        {
            IEnumerable<string> pushPromiseContentPaths = Enumerable.Empty<string>();

            if (_pushPromiseTable.ContainsKey(controller))
            {
                if (_pushPromiseTable[controller].ContainsKey(action))
                {
                    pushPromiseContentPaths = _pushPromiseTable[controller][action];
                }
            }

            return pushPromiseContentPaths;
        }

        internal IEnumerable<string> GetPushPromiseContentPaths()
        {
            var controller = "*";
            var action = "*";
            IEnumerable<string> pushPromiseContentPaths = Enumerable.Empty<string>();

            if (_pushPromiseTable.ContainsKey(controller))
            {
                if (_pushPromiseTable[controller].ContainsKey(action))
                {
                    pushPromiseContentPaths = _pushPromiseTable[controller][action];
                }
            }

            return pushPromiseContentPaths;
        }
    }
}