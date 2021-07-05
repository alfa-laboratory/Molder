using Molder.Web.Models.PageObjects.Attributes;
using Molder.Web.Models.PageObjects.Pages;
using PageObject.Elements.Blocks;

namespace PageObject.Pages.SubPages
{
    [Page(Name = "Add/Remove Elements With Block", Url = "http://{{url}}/add_remove_elements/")]
    public class AddRemoveElementsBlock : Page
    {
        [Block(Name = "Add/Remove Elements", Locator = "//*[@id=\"content\"]/div")]
        AddElementExample addRemoveElements; 
    }
}