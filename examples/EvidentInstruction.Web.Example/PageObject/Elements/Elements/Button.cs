using EvidentInstruction.Web.Models.PageObject.Models.Elements;

namespace PageObject.Elements.Elements
{
    public class Button : Element
    {
        public Button(string name, string locator, string block = null) : base(name, locator, block)
        {
        }
    }
}
