using Molder.Web.Models.PageObjects.Attributes;
using Molder.Web.Models.PageObjects.Elements;
using Molder.Web.Models.PageObjects.Pages;

namespace PageObject.Pages.SubPages
{
    [Page(Name = "Basic Auth", Url = "http://192.168.99.100:9080/basic_auth")]
    public class BasicAuth : Page
    {
        [Element(Name = "Text", Locator = "//*[@id=\"content\"]/div/p")]
        Div text;
    }
}