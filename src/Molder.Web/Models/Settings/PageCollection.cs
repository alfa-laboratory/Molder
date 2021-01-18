using Molder.Web.Helpers;
using System;
using System.Collections.Generic;

namespace Molder.Web.Models.Settings
{
    public class PageCollection
    {
        [ThreadStatic]
        private static IDictionary<string, Type> _pages = null;

        private PageCollection() { }

        public static IDictionary<string, Type> GetPages()
        {
            if (_pages == null)
            {
                _pages = BrowserHelper.CollectPages();
                return _pages;
            }
            return _pages;
        }
    }
}
