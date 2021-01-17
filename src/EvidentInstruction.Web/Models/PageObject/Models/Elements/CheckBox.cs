using EvidentInstruction.Web.Models.PageObject.Models.Abstracts.Elements;

namespace EvidentInstruction.Web.Models.PageObject.Models.Elements
{
    public class CheckBox : BaseClick
    {
        public CheckBox(string name, string locator, bool optional) : base(name, locator, optional) {  }
    }
}
