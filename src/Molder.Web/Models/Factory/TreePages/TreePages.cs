using System;
using System.Collections.Generic;

namespace Molder.Web.Models
{
    public class TreePages
    {
        [ThreadStatic]
        private static IEnumerable<Node> _pages;

        private TreePages() { }

        public static IEnumerable<Node> Get()
        {
            if (_pages == null)
            {
                var pageObject = new PageObject();
                _pages = pageObject.Pages;
            }
            return _pages;
        }
    }
}
