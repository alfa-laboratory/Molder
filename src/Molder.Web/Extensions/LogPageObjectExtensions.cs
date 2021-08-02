using Molder.Web.Models;
using System;
using System.Collections.Generic;

namespace Molder.Web.Extensions
{
    public static class LogPageObjectExtensions
    {
        public static string PageObjectToString(IEnumerable<Node> pages, int level = 0)
        {
            var result = String.Empty;
            for (int i = 0; i < level; i++)
            {
                result += "|   ";
            }
            foreach (Node page in pages)
            {
                if (!(page.Childrens is null))
                {
                    result += Environment.NewLine + "└───" + page.Type.ToString() + "(" + page.Name + ")";
                    PageObjectToString(page.Childrens, level + 1);
                }
                result += Environment.NewLine + "|   " + page.Type.ToString() + "(" + page.Name + ")";
            }
            return result;
        }
    }
}
