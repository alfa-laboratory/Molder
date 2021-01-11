using EvidentInstruction.Web.Models.PageObject.Models.Abstracts.Elements;

namespace EvidentInstruction.Web.Models.PageObject.Models.Elements
{
    public class Button : ClickElement
    {
        public Button(string name, string locator, string block = null) : base(name, locator, block) { }

        public virtual void DoubleClick()
        {
        }

        public virtual void ClickAndHold()
        {
        }
    }
}
