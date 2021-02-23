using System;

namespace Molder.Web.Models.PageObjects.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PageAttribute : Attribute
    {
        public string Name { get; set; }
        public string Url { get; set; } = null;
    }
}