using Molder.Web.Models.PageObject.Models.Elements.Interfaces;
using Molder.Web.Models.PageObject.Models.Mediator;
using Molder.Web.Models.PageObject.Models.Mediator.Interfaces;
using Molder.Web.Models.Providers;
using Molder.Web.Models.Providers.Interfaces;
using Molder.Web.Models.Settings;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Reflection;

namespace Molder.Web.Models.PageObject.Models.Elements
{
    public class Element : IElement
    {
        [ThreadStatic]
        protected IMediator _mediator = null;

        [ThreadStatic]
        protected IDriverProvider _driverProvider;

        protected string _name;
        protected string _locator;

        protected bool _optional;

        [ThreadStatic]
        public IElementProvider _provider = null;

        public Element() { }

        public Element(string name, string locator = null, bool optional = false)
        {
            _name = name;
            _locator = locator;
            _optional = optional;
        }

        public virtual void SetProvider(IDriverProvider provider)
        {
            _driverProvider = provider;
            _provider = null;
            _mediator = new ElementMediator((_driverProvider.Settings as BrowserSetting).ElementTimeout);
            GetElement();
        }

        public string Name => _name;

        public string Text => _mediator.Execute(() => _provider.Text) as string;

        public object Value => _mediator.Execute(() => GetAttribute("value"));

        public string Tag => _mediator.Execute(() => _provider.Tag) as string;

        public bool Loaded => (bool)_mediator.Wait(() => _provider != null);

        public bool Enabled => (bool)_mediator.Wait(() => _provider.Enabled);

        public bool Displayed => (bool)_mediator.Wait(() => _provider.Displayed);

        public bool Selected => (bool)_mediator.Wait(() => _provider.Selected);

        public bool Editabled => (bool)_mediator.Wait(() => _provider.Editabled);

        public string GetAttribute(string name)
        {
            return _mediator.Execute(() => _provider.GetAttribute(name)) as string;
        }

        public void Move()
        {
            var action = new Actions(_driverProvider.GetDriver());
            _mediator.Execute(() => action.MoveToElement(((ElementProvider)_provider).Element).Build().Perform());
        }

        public void PressKey(string key)
        {
            var field = typeof(Keys).GetField(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Static);
            if (Enabled && Displayed)
            {
                _mediator.Execute(() => _provider.SendKeys((string)field?.GetValue(null)));
            }
            else
            {
                throw new ArgumentNullException($"Проверьте, что элемент \"{_name}\" Enabled и Displayed");
            }
        }

        public bool IsTextContains(string text)
        {
            return (bool)_mediator.Wait(() => _provider.TextContain(text));
        }

        public bool IsTextEquals(string text)
        {
            return (bool)_mediator.Wait(() => _provider.TextEqual(text));
        }

        public bool IsTextMatch(string text)
        {
            return (bool)_mediator.Wait(() => _provider.TextMatch(text));
        }

        #region Get webDriver Element

        protected void GetElement()
        {
            _provider = _mediator.Execute(() => _driverProvider.GetElement(By.XPath(_locator))) as IElementProvider;
        }

        #endregion
    }
}