using System.Collections.Generic;
using System.Threading;

namespace Molder.Web.Models
{
    public class TreePages
    {
        private static AsyncLocal<IEnumerable<Node>> _pages = new AsyncLocal<IEnumerable<Node>> { Value = null };

        private TreePages() { }

        public static IEnumerable<Node> Get()
        {
            if (_pages.Value == null)
            {
                var pageObject = new PageObject();
                _pages.Value = pageObject.Pages;
            }
            return _pages.Value;
        }
    }
}
