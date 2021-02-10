using Molder.Web.Models.PageObjects.Attributes;
using Molder.Web.Models.PageObjects.Blocks;
using Molder.Web.Models.PageObjects.Elements;

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
