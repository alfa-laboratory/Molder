using Molder.Web.Models.PageObjects.Attributes;
using Molder.Web.Models.PageObjects.Blocks;

namespace PageObject.Elements.Blocks
{
    public class Content : Block
    {
        public Content(string name, string locator, bool optional) : base(name, locator, optional) {   }

        [Block(Name = "row 1", Locator = "/div[1]")]
        FrsContent fContent;
        
        [Block(Name = "row 2", Locator = "/div[2]")]
        ScndContent scndContent;
        
        [Collection(Name = "subContent", Locator = "/*[@class=\"row\"]")]
        SubContent subContent;
    }
}