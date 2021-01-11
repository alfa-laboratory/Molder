using EvidentInstruction.Web.Models.PageObject.Models.Elements;

namespace EvidentInstruction.Web.Models.PageObject.Models.Abstracts.Elements
{
    public abstract class ClickElement : Element
    {
        public ClickElement(string name, string locator, string block = null) : base(name, locator, block) { }

        public virtual void Click()
        {
        }
    }
}
