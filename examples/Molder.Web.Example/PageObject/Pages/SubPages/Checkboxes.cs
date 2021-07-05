using Molder.Web.Models.PageObjects.Attributes;
using Molder.Web.Models.PageObjects.Elements;
using Molder.Web.Models.PageObjects.Pages;

namespace PageObject.Pages.SubPages
{
    [Page(Name = "Checkboxes", Url = "http://{{url}}/checkboxes")]
    public class Checkboxes : Page
    {
        [Element(Name = "checkbox 1", Locator = "//*[@id=\"checkboxes\"]/input[1]")]
        CheckBox checkbox1;

        [Element(Name = "checkbox 2", Locator = "//*[@id=\"checkboxes\"]/input[2]")]
        CheckBox checkbox2;
    }
}