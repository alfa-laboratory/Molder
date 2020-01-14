using System;

namespace AlfaBank.AFT.Core.Models.Web.Attributes
{
    public class ElementAttribute : Attribute
    {
        public string Name { get; set; }
        public string Locator { get; set; }
        public bool Hidden { get; set; } = false;
        public bool Optional { get; set; } = false;
    }
}
