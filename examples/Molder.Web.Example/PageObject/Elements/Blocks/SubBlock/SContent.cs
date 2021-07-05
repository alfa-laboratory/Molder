using Molder.Web.Models.PageObjects.Attributes;
using Molder.Web.Models.PageObjects.Blocks;
using Molder.Web.Models.PageObjects.Elements;

namespace PageObject.Elements.Blocks
{
    public class ScndContent : Block
    {
        public ScndContent(string name, string locator, bool optional) : base(name, locator, optional) {   }

        [Element(Name = "Text", Locator = "//*[@id=\"content\"]/div[2]/div[2]")]
        Div text;
    }
}
