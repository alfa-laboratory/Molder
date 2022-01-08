using System;
using Molder.Web.Infrastructures;

namespace Molder.Web.Models.PageObjects.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PageAttribute : Attribute
    {
        public string Name { get; set; }
        public string Url { get; set; } = default;
    }
    
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FrameAttribute : ElementAttribute
    {
        public int? Number { get; set; } = default;
        public string FrameName { get; set; } = default;
    }
    
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ElementAttribute : Attribute
    {
        public string Name { get; set; }

        public How How { get; set; } = How.XPath;
        public string Locator { get; set; }
        public bool Optional { get; set; } = default;
    }
    
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class BlockAttribute : ElementAttribute { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class CollectionAttribute : ElementAttribute { }
}