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
        protected AsyncLocal<IMediator> _mediator = new AsyncLocal<IMediator> { Value = null };

        protected IDriverProvider _driverProvider;
        public IElementProvider _provider = null;

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
            _provider = null;
            _mediator.Value = new ElementMediator((_driverProvider.Settings as BrowserSetting).ElementTimeout);
            GetElement();
        }

        public Node Root { get; set; }

        public string Name { get; set; }
        public bool Optional { get; set; }
        public string Locator { get; set; }

        public string Text => _mediator.Value.Execute(() => _provider.Text) as string;

        public object Value => _mediator.Value.Execute(() => GetAttribute("value"));

        public string Tag => _mediator.Value.Execute(() => _provider.Tag) as string;

        public bool Loaded => (bool)_mediator.Value.Wait(() => _provider != null);

        public bool Enabled => (bool)_mediator.Value.Wait(() => _provider.Enabled);

        public bool Displayed => (bool)_mediator.Value.Wait(() => _provider.Displayed);

        public bool Selected => (bool)_mediator.Value.Wait(() => _provider.Selected);

        public bool Editabled => (bool)_mediator.Value.Wait(() => _provider.Editabled);

        public string GetAttribute(string name)
        {
            return _mediator.Value.Execute(() => _provider.GetAttribute(name)) as string;
        }

        public void Move()
        {
            var action = new Actions(_driverProvider.GetDriver());
            _mediator.Value.Execute(() => action.MoveToElement(((ElementProvider)_provider).Element).Build().Perform());
        }

        public void PressKey(string key)
        {
            var field = typeof(Keys).GetField(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Static);
            if (Enabled && Displayed)
            {
                _mediator.Value.Execute(() => _provider.SendKeys((string)field?.GetValue(null)));
            }
            else
            {
                throw new ArgumentNullException($"Проверьте, что элемент \"{Name}\" Enabled и Displayed");
            }
        }

        public bool IsTextContains(string text)
        {
            return (bool)_mediator.Value.Wait(() => _provider.TextContain(text));
        }

        public bool IsTextEquals(string text)
        {
            return (bool)_mediator.Value.Wait(() => _provider.TextEqual(text));
        }

        public bool IsTextMatch(string text)
        {
            return (bool)_mediator.Value.Wait(() => _provider.TextMatch(text));
        }

        #region Get webDriver Element

        protected void GetElement(string locator = null)
        {
            _provider = _mediator.Value.Execute(() => _driverProvider.GetElement(By.XPath(locator ?? Locator))) as IElementProvider;
        }

        #endregion
    }
}