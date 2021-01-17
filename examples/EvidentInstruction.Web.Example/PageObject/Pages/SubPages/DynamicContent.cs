using EvidentInstruction.Web.Models.PageObject.Attributes;
using EvidentInstruction.Web.Models.PageObject.Models.Page;
using PageObject.Elements.Blocks;

namespace PageObject.Pages.SubPages
{
    [Page(Name = "Dynamic Content", Url = "http://192.168.99.100:7080/dynamic_content")]
    public class DynamicContent : Page
    {
        [Block(Name = "Content", Locator = "//*[@id=\"content\"]")]
        Content content;
    }
}