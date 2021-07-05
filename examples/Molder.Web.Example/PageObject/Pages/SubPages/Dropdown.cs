using Molder.Web.Models.PageObjects.Attributes;
using Molder.Web.Models.PageObjects.Elements;
using Molder.Web.Models.PageObjects.Pages;

namespace PageObject.Pages.SubPages
{
    [Page(Name = "Dropdown", Url = "http://{{url}}/dropdown")]
    public class DropdownPage : Page
    {
        [Element(Name = "Dropdown List", Locator = "//*[@id=\"dropdown\"]")]
        Dropdown dropdownList;
    }
}