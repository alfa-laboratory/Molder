using Molder.Web.Models.PageObject.Attributes;
using Molder.Web.Models.PageObject.Models.Elements;
using Molder.Web.Models.PageObject.Models.Page;

namespace PageObject.Pages.SubPages
{
    [Page(Name = "Add/Remove Elements", Url = "http://192.168.99.100:7080/add_remove_elements/")]
    public class AddRemoveElements : Page
    {
        [Element(Name = "Add Element", Locator = "//*[@id=\"content\"]/div/button")]
        Button addElement;

        [Element(Name = "Delete", Locator = "//*[@id=\"elements\"]/button[1]", Optional = true)]
        Button delete;
    }
}