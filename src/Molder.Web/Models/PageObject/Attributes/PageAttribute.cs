using System;

namespace Molder.Web.Models.PageObject.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PageAttribute : Attribute
    {
        public string Name { get; set; }
        public string Url { get; set; } = null;
    }
}