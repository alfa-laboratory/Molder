using EvidentInstruction.Web.Models.PageObject.Attributes;
using EvidentInstruction.Web.Models.PageObject.Models.Blocks;
using EvidentInstruction.Web.Models.PageObject.Models.Elements;

namespace PageObject.Elements.Blocks
{
    public class AddElementExample : Block
    {
        public AddElementExample(string name, string locator, bool optional) : base(name, locator, optional) {   }

        [Element(Name = "Add Element", Locator = "//*[@id=\"content\"]/div/button")]
        Button addElement;

        [Element(Name = "Delete", Locator = "//*[@id=\"elements\"]/button[1]", Optional = true)]
        Button delete;
    }
}
