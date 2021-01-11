using EvidentInstruction.Web.Models.PageObject.Models.Abstracts.Elements;

namespace EvidentInstruction.Web.Models.PageObject.Models.Elements
{
    public class CheckBox : ClickElement
    {
        public CheckBox(string name, string locator, string block = null) : base(name, locator, block) {  }
    }
}
