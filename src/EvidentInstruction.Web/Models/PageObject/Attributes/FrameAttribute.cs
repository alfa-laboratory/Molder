using System;

namespace EvidentInstruction.Web.Models.PageObject.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FrameAttribute : ElementAttribute
    {
        public int? Number { get; set; } = null;
        public string FrameName { get; set; } = null;
    }
}
