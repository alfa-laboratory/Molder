using EvidentInstruction.Web.Models.Providers.Interfaces;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace EvidentInstruction.Web.Models.Providers
{
    public class ElementProvider : IElementProvider
    {
        [ThreadStatic]
        public IWebElement Element = null;

        public bool Displayed => Element.Displayed;

        public bool Selected => Element.Selected;

        public bool Enabled => Element.Enabled;

        public Point Location => Element.Location;

        public string Text => Element.Text;

        public string Tag => Element.TagName;

        public void Clear()
        {
            Element.Clear();
        }

        public void Click()
        {
            Element.Click();
        }

        public IElementProvider FindElement(By by)
        {
            var element = Element.FindElement(by);
            return new ElementProvider()
            {
                Element = element
            };
        }

        public ReadOnlyCollection<IElementProvider> FindElements(By by)
        {
            var elements = Element.FindElements(by);
            var listElement = new List<IElementProvider>();
            foreach (var element in elements)
            {
                listElement.Add(new ElementProvider()
                {
                    Element = element
                });
            }
            return listElement.AsReadOnly();
        }

        public string GetAttribute(string name)
        {
            return Element.GetAttribute(name);
        }

        public string GetCss(string name)
        {
            return Element.GetCssValue(name);
        }

        public void SendKeys(string keys)
        {
            Element.SendKeys(keys);
        }
    }
}
