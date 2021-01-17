using EvidentInstruction.Web.Models.PageObject.Models.Abstracts.Elements;
using System;

namespace EvidentInstruction.Web.Models.PageObject.Models.Elements
{
    public class A : BaseClick
    {
        public A(string name, string locator, bool optional = false) : base(name, locator, optional) { }

        public string Href { get => GetHref(); }

        private string GetHref()
        {
            if (Enabled && Displayed)
            {
                return (string)_mediator.Execute(() => _provider.GetAttribute("href"));
            }
            else
            {
                throw new ArgumentException($"Проверьте, что элемент \"{_name}\" Enabled и Displayed");
            }
        }
    }
}
