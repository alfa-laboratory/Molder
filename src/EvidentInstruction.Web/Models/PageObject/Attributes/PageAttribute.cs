using System;

namespace EvidentInstruction.Web.Models.PageObject.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PageAttribute : FrameAttribute
    {
        public string Url { get; set; }
    }
}
