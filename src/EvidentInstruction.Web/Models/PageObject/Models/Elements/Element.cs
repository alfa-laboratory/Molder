using EvidentInstruction.Web.Models.PageObject.Models.Elements.Interfaces;
using EvidentInstruction.Web.Models.PageObject.Models.Mediator;
using EvidentInstruction.Web.Models.PageObject.Models.Mediator.Interfaces;
using EvidentInstruction.Web.Models.Providers;
using EvidentInstruction.Web.Models.Providers.Interfaces;
using EvidentInstruction.Web.Models.Settings;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Reflection;

namespace EvidentInstruction.Web.Models.PageObject.Models.Elements
{
    public class Element : IElement
    {
        [ThreadStatic]
        private IMediator _mediator = null;

        [ThreadStatic]
        protected IDriverProvider _driverProvider;

        protected string _name;
        protected string _locator;

        protected string _block;

        [ThreadStatic]
        public ElementProvider _provider = null;

        public Element(string name, string locator, string block = null)
        {
            _name = name;
            _locator = locator;
            _block = block;
        }

        public void SetProvider(IDriverProvider provider)
        {
            _driverProvider = provider;
            _mediator = new ElementMediator((_driverProvider.Settings as BrowserSetting).ElementTimeout);
            _provider = new ElementProvider((_driverProvider.Settings as BrowserSetting).ElementTimeout);
        }

        public string Name => _name;

        public string Text => _mediator.Execute(() => GetElement().Text) as string;

        public object Value => _mediator.Execute(() => GetAttribute("value"));

        public bool Loaded => (bool)_mediator.Wait(() => GetElement() != null);

        public bool Enabled => (bool)_mediator.Wait(() => GetElement().Enabled);

        public bool Displayed => (bool)_mediator.Wait(() => GetElement().Displayed);

        public bool Selected => (bool)_mediator.Wait(() => GetElement().Selected);

        public bool Editabled => (bool)_mediator.Wait(() => GetElement().Editabled);

        public string GetAttribute(string name)
        {
            return _mediator.Execute(() => GetElement().GetAttribute(name)) as string;
        }

        public void Move()
        {
            var action = new Actions(_driverProvider.GetDriver());
            _mediator.Execute(() => action.MoveToElement(((ElementProvider)GetElement()).Element).Build().Perform());
        }

        public void PressKey(string key)
        {
            var field = typeof(Keys).GetField(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Static);
            if (Enabled && Displayed)
            {
                _mediator.Execute(() => GetElement().SendKeys((string)field?.GetValue(null)));
            }
            else
            {
                throw new ArgumentNullException($"Проверьте, что элемент \"{_name}\" Enabled и Displayed");
            }
        }

        public bool IsTextContains(string text)
        {
            return (bool)_mediator.Wait(() => GetElement().TextContain(text));
        }

        public bool IsTextEquals(string text)
        {
            return (bool)_mediator.Wait(() => GetElement().TextEqual(text));
        }

        public bool IsTextMatch(string text)
        {
            return (bool)_mediator.Wait(() => GetElement().TextMatch(text));
        }

        #region Get webDriver Element

        private IElementProvider GetElement()
        {
            return _mediator.Execute(() => _driverProvider.GetElement(By.XPath(_locator))) as IElementProvider;
        }

        #endregion
    }
}