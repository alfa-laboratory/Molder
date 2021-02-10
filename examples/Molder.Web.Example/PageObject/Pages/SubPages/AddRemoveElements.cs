using Molder.Web.Models.PageObjects.Attributes;
using Molder.Web.Models.PageObjects.Elements;
using Molder.Web.Models.PageObjects.Pages;

namespace PageObject.Pages.SubPages
{
    [Page(Name = "Add/Remove Elements", Url = "http://192.168.99.100:9080/add_remove_elements/")]
    public class AddRemoveElements : Page
    {
        [Element(Name = "Add Element", Locator = "//*[@id=\"content\"]/div/button")]
        Button addElement;

        [Element(Name = "Delete", Locator = "//*[@id=\"elements\"]/button[1]", Optional = true)]
        Button delete;
    }
}