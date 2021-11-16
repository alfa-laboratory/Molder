using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace Selenium.WebDriver.WaitExtensions.WaitConditions
{
    public class AttributeWaitConditions : WaitConditionsBase, IAttributeWaitConditions
    {
        private readonly IWebElement _webelement;

        public AttributeWaitConditions(IWebElement webelement, int delayMs) : base(delayMs)
        {
            _webelement = webelement;
        }

        public bool ToContain(string attrName)
        {
            return WaitFor(() => !string.IsNullOrEmpty(_webelement.GetAttribute(attrName)), GetAttributesString());
        }

        public bool ToNotContain(string attrName)
        {
            return WaitFor(() => !ToContain(attrName), GetAttributesString());
        }

        public bool ToContainWithValue(string attrName, string attrValue)
        {
            return WaitFor(() => ToContain(attrName) && _webelement.GetAttribute(attrName) == attrValue, GetAttributesString());
        }

        public bool ToContainWithoutValue(string attrName, string attrValue)
        {
            return WaitFor(() => !ToContain(attrName) && _webelement.GetAttribute(attrName) != attrValue, GetAttributesString());
        }


        private IDictionary<string, object> GetElementAttributes()
        {
            var driver = ((IWrapsDriver) _webelement).WrappedDriver;

            return ((IJavaScriptExecutor)driver).ExecuteScript("var items = {}; for (index = 0; index < arguments[0].attributes.length; ++index) { items[arguments[0].attributes[index].name] = arguments[0].attributes[index].value }; return items;", _webelement) as Dictionary<string, object>;
        }

        private string GetAttributesString()
        {
            var elementAttributes = GetElementAttributes();
            string attrsString = "";

            if (elementAttributes.Count > 0) attrsString = string.Join("\n   ",elementAttributes.Keys.Select(k => k + " = " + elementAttributes[k]));

            return $"attribute list;{attrsString}";
        }
    }
}