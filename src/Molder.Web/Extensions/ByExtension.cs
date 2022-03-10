using System;
using Molder.Web.Infrastructures;
using OpenQA.Selenium;

namespace Molder.Web.Extensions
{
    public static class ByExtension
    {
        public static By GetBy(this How how, string _using) =>
            how switch
            {
                How.Id => By.Id(_using),
                How.Name => By.Name(_using),
                How.TagName => By.TagName(_using),
                How.ClassName => By.ClassName(_using),
                How.CssSelector => By.CssSelector(_using),
                How.LinkText => By.LinkText(_using),
                How.PartialLinkText => By.PartialLinkText(_using),
                How.XPath => By.XPath(_using),
                _ => throw new ArgumentOutOfRangeException(nameof(how), how, null)
            };
    }
}