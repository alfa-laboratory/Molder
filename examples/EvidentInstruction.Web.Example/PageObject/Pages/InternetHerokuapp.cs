using EvidentInstruction.Web.Models.PageObject.Attributes;
using EvidentInstruction.Web.Models.PageObject.Models.Elements;
using EvidentInstruction.Web.Models.PageObject.Models.Page;

namespace PageObject.Pages
{
    [Page(Name = "InternetHerokuapp", Url = "http://192.168.99.100:7080/")]
    public class InternetHerokuapp : Page
    {
        [Element(Name = "Add/Remove Elements", Locator = "//*[@id=\"content\"]/ul/li[2]/a")]
        A addRemoveElements;

        [Element(Name = "Checkboxes", Locator = "//*[@id=\"content\"]/ul/li[6]/a")]
        A checkboxes;

        [Element(Name = "Dropdown", Locator = "//*[@id=\"content\"]/ul/li[11]/a")]
        A dropdown;

        [Element(Name = "Dynamic Content", Locator = "//*[@id=\"content\"]/ul/li[12]/a")]
        A dynamicContent;

        [Element(Name = "Frames", Locator = "//*[@id=\"content\"]/ul/li[22]/a")]
        A frames;

        [Element(Name = "Inputs", Locator = "//*[@id=\"content\"]/ul/li[27]/a")]
        A inputs;
    }
}
