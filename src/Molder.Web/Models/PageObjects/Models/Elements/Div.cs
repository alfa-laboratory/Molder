﻿using Molder.Web.Infrastructures;

namespace Molder.Web.Models.PageObjects.Elements
{
    public class Div : Element
    {
        public Div(string name, string locator, bool optional) : base(name, locator, optional) { }
    }

    public class Default : Element
    {
        public Default(string name, string locator, bool optional) : base(name, locator, optional) { }

        public Default(How how, string locator) : base(how, locator){ }
    }
}