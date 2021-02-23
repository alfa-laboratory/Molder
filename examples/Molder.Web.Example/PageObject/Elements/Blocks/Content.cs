using Molder.Web.Models.PageObjects.Attributes;
using Molder.Web.Models.PageObjects.Blocks;

namespace PageObject.Elements.Blocks
{
    public class Content : Block
    {
        public Content(string name, string locator, bool optional) : base(name, locator, optional) {   }

        [Block(Name = "row 1", Locator = "//*[@id=\"content\"]/div[1]")]
        FrsContent fContent;
    }
}
