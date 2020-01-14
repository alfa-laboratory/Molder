using System;

namespace AlfaBank.AFT.Core.Models.Web.Attributes
{
    public class PageAttribute : Attribute
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
