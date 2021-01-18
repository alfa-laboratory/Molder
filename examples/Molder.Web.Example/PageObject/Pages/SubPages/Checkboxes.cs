using Molder.Web.Models.PageObject.Attributes;
using Molder.Web.Models.PageObject.Models.Elements;
using Molder.Web.Models.PageObject.Models.Page;

namespace PageObject.Pages.SubPages
{
    [Page(Name = "Checkboxes", Url = "http://192.168.99.100:7080/checkboxes")]
    public class Checkboxes : Page
    {
        [Element(Name = "checkbox 1", Locator = "//*[@id=\"checkboxes\"]/input[1]")]
        CheckBox checkbox1;

        [Element(Name = "checkbox 2", Locator = "//*[@id=\"checkboxes\"]/input[2]")]
        CheckBox checkbox2;
    }
}