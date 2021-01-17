using System;

namespace EvidentInstruction.Web.Models.PageObject.Models.Elements
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
                throw new ArgumentException($"Проверьте, что элемент \"{_name}\" имеет тип file");
            }
        }
    }
}
