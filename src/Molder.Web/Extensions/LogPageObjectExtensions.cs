using Molder.Web.Models;
using System;
using System.Collections.Generic;

namespace Molder.Web.Extensions
{
    public static class LogPageObjectExtensions
    {
        public static string PageObjectToString(this IEnumerable<Node> pages, int level = 0)
        {
            var result = string.Empty;
            if (level > 0) result += Environment.NewLine;
            var stringlevel = string.Empty;
            for (var i = 0; i < level; i++)
            {
                stringlevel += "|   ";
            }
            foreach (var page in pages)
            {
                if (page.Childrens is not null)
                {
                    result += $"{stringlevel}└───{page.Type}({page.Name})";
                    result += PageObjectToString(page.Childrens, level + 1);
                }
                else result += $"{stringlevel}|   {page.Type}({page.Name}){Environment.NewLine}";
            }
            return result;
        }
    }
}