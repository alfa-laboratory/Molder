﻿using System;

namespace Molder.Web.Models.PageObjects.Elements
{
    public class A : DefaultClick
    {
        public A(string name, string locator, bool optional = false) : base(name, locator, optional) { }

        public string Href => GetHref();

        private string GetHref()
        {
            if (Enabled && Displayed)
            {
                return (string)mediator.Execute(() => ElementProvider.GetAttribute("href"));
            }

            throw new ArgumentException($"Проверьте, что элемент \"{Name}\" Enabled и Displayed");
        }
    }
}