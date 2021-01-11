using EvidentInstruction.Web.Models.PageObject.Models.Elements;

namespace PageObject.Elements.Elements
{
    public class Input : Element
    {
        public Input(string name, string locator, string block = null) : base(name, locator, block)  { }
    }
}
