using Molder.Web.Models.Mediator;
using Molder.Web.Models.Providers;
using Molder.Web.Models.Settings;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Reflection;
using System.Threading;

namespace Molder.Web.Models.PageObjects.Elements
{
    public class Element : IElement
    {
        #region Element Mediator

        private AsyncLocal<IMediator> _elementMediator = new AsyncLocal<IMediator>{ Value = null };

        protected IMediator mediator
        {
            get => _elementMediator.Value;
            set => _elementMediator.Value = value;
        }

        #endregion
        
        protected IDriverProvider _driverProvider;
        protected IElementProvider _provider = null;

        public Element() { }

        public Element(string name, string locator = null, bool optional = false)
        {
            Name = name;
            Locator = locator;
            Optional = optional;
        }

        public virtual void SetProvider(IDriverProvider provider)
        {
            _driverProvider = provider;
            mediator = new ElementMediator(BrowserSettings.Settings.Timeout);
            
            _provider = GetElementBy();
        }

        public Node Root { get; set; }

        public string Name { get; set; }
        public bool Optional { get; set; }
        public string Locator { get; set; }

        public string Text => mediator.Execute(() => _provider.Text) as string;

        public object Value => mediator.Execute(() => GetAttribute("value"));

        public string Tag => mediator.Execute(() => _provider.Tag) as string;

        public bool Loaded => (bool)mediator.Wait(() => _provider != null);
        public bool NotLoaded => (bool)mediator.Wait(() => _provider == null);

        public bool Enabled => (bool)mediator.Wait(() => _provider.Enabled);
        public bool Disabled => (bool)mediator.Wait(() => _provider.Disabled);

        public bool Displayed => (bool)mediator.Wait(() => _provider.Displayed);
        public bool NotDisplayed => (bool)mediator.Wait(() => _provider.NotDisplayed);

        public bool Selected => (bool)mediator.Wait(() => _provider.Selected);
        public bool NotSelected => (bool)mediator.Wait(() => _provider.NotSelected);

        public bool Editabled => (bool)mediator.Wait(() => _provider.Editabled);
        public bool NotEditable => (bool)mediator.Wait(() => _provider.NotEditabled);

        public string GetAttribute(string name)
        {
            return mediator.Execute(() => _provider.GetAttribute(name)) as string;
        }

        public void Move()
        {
            var action = new Actions(_driverProvider.GetDriver());
            mediator.Execute(() => action.MoveToElement(((ElementProvider)_provider).WebElement).Build().Perform());
        }

        public void PressKey(string key)
        {
            var field = typeof(Keys).GetField(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Static);
            if (Enabled && Displayed)
            {
                mediator.Execute(() => _provider.SendKeys((string)field?.GetValue(null)));
            }
            else
            {
                throw new ArgumentNullException($"Проверьте, что элемент \"{Name}\" Enabled и Displayed");
            }
        }

        public bool IsTextContains(string text)
        {
            return (bool)mediator.Wait(() => _provider.TextContain(text));
        }

        public bool IsTextEquals(string text)
        {
            return (bool)mediator.Wait(() => _provider.TextEqual(text));
        }

        public bool IsTextMatch(string text)
        {
            return (bool)mediator.Wait(() => _provider.TextMatch(text));
        }

        #region Get webDriver Element

        protected void GetElement(string locator = null)
        {
            _provider = GetElementBy(locator);
        }

        private IElementProvider GetElementBy(string locator = null)
        {
            return mediator.Execute(() => _driverProvider.GetElement(By.XPath(locator ?? Locator))) as
                IElementProvider;
        }

        #endregion
    }
}