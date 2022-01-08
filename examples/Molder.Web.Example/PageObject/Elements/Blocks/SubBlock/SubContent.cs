using Molder.Web.Models.PageObjects.Attributes;
using Molder.Web.Models.PageObjects.Blocks;
using Molder.Web.Models.PageObjects.Elements;

namespace PageObject.Elements.Blocks
{
    public class SubContent : Block
    {
        public SubContent(string name, string locator, bool optional) : base(name, locator, optional) { }
        
        [Element(Name = "Image", Locator = "/div[1]")]
        Div image;
        [Element(Name = "Text", Locator = "/div[2]")]
        Div text;
    }
}