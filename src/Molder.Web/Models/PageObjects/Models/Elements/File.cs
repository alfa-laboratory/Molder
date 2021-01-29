using System;

namespace Molder.Web.Models.PageObjects.Elements
{
    public class File : Input
    {
        public File(string name, string locator, bool optional) : base(name, locator, optional) { }

        public override void SetText(string text)
        {
            if (Tag == "file")
            {
                _mediator.Execute(() => _provider.SendKeys(text));
            }
            else
            {
                throw new ArgumentException($"Проверьте, что элемент \"{Name}\" имеет тип file");
            }
        }
    }
}
