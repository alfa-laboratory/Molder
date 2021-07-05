using Molder.Web.Models.PageObjects.Attributes;
using Molder.Web.Models.PageObjects.Elements;
using Molder.Web.Models.PageObjects.Pages;

namespace PageObject.Pages.SubPages
{
    [Page(Name = "Frames", Url = "http://{{url}}/frames")]
    public class Frames : Page
    {
        [Element(Name = "Nested Frames", Locator = "//*[@id=\"content\"]/div/ul/li[1]/a")]
        A nestedFrames;
    }
}