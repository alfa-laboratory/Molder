using Molder.Web.Models.PageObject.Attributes;
using Molder.Web.Models.PageObject.Models.Elements;
using Molder.Web.Models.PageObject.Models.Page;

namespace PageObject.Pages.SubPages
{
    [Page(Name = "Frames", Url = "http://192.168.99.100:7080/frames")]
    public class Frames : Page
    {
        [Element(Name = "Nested Frames", Locator = "//*[@id=\"content\"]/div/ul/li[1]/a")]
        A nestedFrames;
    }
}