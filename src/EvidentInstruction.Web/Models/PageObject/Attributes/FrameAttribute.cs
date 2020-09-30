using System;

namespace EvidentInstruction.Web.Models.PageObject.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FrameAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
