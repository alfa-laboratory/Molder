﻿using System;

namespace Molder.Web.Models.PageObjects.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ElementAttribute : Attribute
    {
        public string Name { get; set; }
        public string Locator { get; set; }
        public bool Optional { get; set; } = false;
    }
}
