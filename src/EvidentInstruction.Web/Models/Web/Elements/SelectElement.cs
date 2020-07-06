using System;

namespace AlfaBank.AFT.Core.Models.Web.Elements
{
    public class SelectElement : Element
    {
        public SelectElement(string name, string xpath) : base(name, xpath) { }

        public virtual void SelectByValue(string value)
        {
            if (IsEnabled() && IsVisible())
            {
                var element = GetWebElement();

                var selectElement = new OpenQA.Selenium.Support.UI.SelectElement(element);
                selectElement.SelectByValue(value);
            }
            else
            {
                throw new ArgumentNullException($"Элемент \"{_name}\" не найден на странице");
            }
        }

        public virtual void SelectByName(string text)
        {
            if (IsEnabled() && IsVisible())
            {
                var element = GetWebElement();

                var selectElement = new OpenQA.Selenium.Support.UI.SelectElement(element);
                selectElement.SelectByText(text);
            }
            else
            {
                throw new ArgumentNullException($"Элемент \"{_name}\" не найден на странице");
            }
        }
    }
}