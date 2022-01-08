using Molder.Web.Models.Mediator;
using Molder.Web.Models.Providers;
using Molder.Web.Models.Settings;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Molder.Web.Exceptions;
using Molder.Web.Extensions;
using Molder.Web.Infrastructures;

namespace Molder.Web.Models.PageObjects.Elements
{
    public abstract class Element : IElement, ICloneable
    {
        private AsyncLocal<IMediator> _elementMediator = new() { Value = null };
        protected IMediator mediator
        {
            get => _elementMediator.Value;
            set => _elementMediator.Value = value;
        }
        
        public IDriverProvider Driver { get; set; } = default;
        public IElementProvider ElementProvider { get; set; } = default;

        public Node Root { get; set; }
        public string Name { get; set; }
        public How How { get; set; } = How.XPath;
        public string Locator { get; set; }

        public By By => How.GetBy(Locator);

        public bool Optional { get; set; }

        public Element(string name, string locator, bool optional = false)
        {
            Name = name;
            Locator = locator;
            Optional = optional;
        }

        public Element(How how, string locator)
        {
            Locator = locator;
            How = how;
        }

        public string Text => mediator.Execute(() => ElementProvider.Text) as string;
        public object Value => mediator.Execute(() => GetAttribute("value"));
        public string Tag => mediator.Execute(() => ElementProvider.Tag) as string;
        public bool Loaded => (bool)mediator.Wait(() => ElementProvider != null);
        public bool NotLoaded => (bool)mediator.Wait(() => ElementProvider == null);
        public bool Enabled => (bool)mediator.Wait(() => ElementProvider.Enabled);
        public bool Disabled => (bool)mediator.Wait(() => !ElementProvider.Enabled);
        public bool Displayed => (bool)mediator.Wait(() => ElementProvider.Displayed);
        public bool NotDisplayed => (bool)mediator.Wait(() => !ElementProvider.Displayed);
        public bool Selected => (bool)mediator.Wait(() => ElementProvider.Selected);
        public bool NotSelected => (bool)mediator.Wait(() => !ElementProvider.Selected);
        public bool Editabled => (bool)mediator.Wait(IsEditabled);
        public bool NotEditable => (bool)mediator.Wait(() => !IsEditabled());

        public void SetProvider(IDriverProvider provider)
        {
            Driver = provider;
            mediator = new ElementMediator(BrowserSettings.Settings.Timeout);
        }

        public void Get()
        {
            ElementProvider = mediator.Execute(() => Driver.GetElement(Locator, How)) as
                IElementProvider;
        }

        public IElement Find(string locator, How how = How.XPath)
        {
            var by = how.GetBy(locator);
            return new Default(how, locator)
            {
                ElementProvider = ElementProvider.FindElement(by)
            };
        }

        public IEnumerable<IElement> FindAll(string locator, How how = How.XPath)
        {
            var by = how.GetBy(locator);
            var elements = ElementProvider.FindElements(by);
            var listElement = elements.Select(element => new Default(how, locator) {ElementProvider = element}).Cast<IElement>().ToList();
            return listElement.AsReadOnly();
        }
        
        public void Clear()
        {
            try
            {
                ElementProvider.Clear();
            }
            catch (Exception ex)
            {
                throw new ElementException($"Clear element is return error with message {ex.Message}");
            }
        }

        public string GetAttribute(string name)
        {
            return mediator.Execute(() => ElementProvider.GetAttribute(name)) as string;
        }
        public void Move()
        {
            var action = new Actions(Driver.GetDriver());
            mediator.Execute(() => action.MoveToElement(((ElementProvider)ElementProvider).WebElement).Build().Perform());
        }
        public void PressKeys(string keys)
        {
            var field = typeof(Keys).GetField(keys, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Static);
            if (Enabled && Displayed)
            {
                mediator.Execute(() => ElementProvider.SendKeys((string)field?.GetValue(null)));
            }
            else
            {
                throw new ArgumentNullException($"Проверьте, что элемент \"{Name}\" Enabled и Displayed");
            }
        }
        
        public bool IsTextContains(string text)
        {
            return (bool)mediator.Wait(() => ElementProvider.TextContain(text));
        }

        public bool IsTextEquals(string text)
        {
            return (bool)mediator.Wait(() => ElementProvider.TextEqual(text));
        }

        public bool IsTextMatch(string text)
        {
            return (bool)mediator.Wait(() => ElementProvider.TextMatch(text));
        }

        public void WaitUntilAttributeValueEquals(string attributeName, string attributeValue)
        {
            mediator.Execute(() => ElementProvider.WaitUntilAttributeValueEquals(attributeName, attributeValue));
        }
        
        private bool IsEditabled()
        {
            return Convert.ToBoolean(GetAttribute("readonly"));
        }
        
        #region Get webDriver Element

        protected void GetElement(string locator, How how = How.XPath)
        {
            ElementProvider = GetElementBy(locator, how);
        }

        private IElementProvider GetElementBy(string locator, How how = How.XPath)
        {
            return mediator.Execute(() => Driver.GetElement(locator, how)) as
                IElementProvider;
        }

        #endregion

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}