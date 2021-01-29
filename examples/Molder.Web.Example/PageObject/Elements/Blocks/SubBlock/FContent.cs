using Molder.Web.Models.PageObjects.Attributes;
using Molder.Web.Models.PageObjects.Blocks;
using Molder.Web.Models.PageObjects.Elements;

namespace PageObject.Elements.Blocks
{
    public class FrsContent : Block
    {
        public FrsContent(string name, string locator, bool optional) : base(name, locator, optional) {   }

        [Element(Name = "Text", Locator = "//*[@id=\"content\"]/div[1]/div[2]")]
        Div text;
    }
}
