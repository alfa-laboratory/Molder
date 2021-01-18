using Molder.Web.Models.PageObject.Attributes;
using Molder.Web.Models.PageObject.Models.Page;
using PageObject.Elements.Blocks;

namespace PageObject.Pages.SubPages
{
    [Page(Name = "Add/Remove Elements With Block", Url = "http://192.168.99.100:7080/add_remove_elements/")]
    public class AddRemoveElementsBlock : Page
    {
        [Block(Name = "Add/Remove Elements", Locator = "//*[@id=\"content\"]/div")]
        AddElementExample addRemoveElements; 
    }
}