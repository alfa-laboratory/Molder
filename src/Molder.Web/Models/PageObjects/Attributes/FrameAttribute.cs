using System;

namespace Molder.Web.Models.PageObjects.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FrameAttribute : ElementAttribute
    {
        public int? Number { get; set; } = null;
        public string FrameName { get; set; } = null;
    }
}
