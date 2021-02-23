using Molder.Web.Models.PageObjects.Attributes;
using Molder.Web.Models.PageObjects.Pages;
using PageObject.Elements.Blocks;

namespace PageObject.Pages.SubPages
{
    [Page(Name = "Dynamic Content", Url = "http://192.168.99.100:9080/dynamic_content")]
    public class DynamicContent : Page
    {
        [Block(Name = "Content", Locator = "//*[@id=\"content\"]")]
        Content content;
    }
}