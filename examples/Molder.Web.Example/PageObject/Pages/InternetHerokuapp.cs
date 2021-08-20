using Molder.Web.Models.PageObjects.Attributes;
using Molder.Web.Models.PageObjects.Elements;
using Molder.Web.Models.PageObjects.Pages;

namespace PageObject.Pages
{
    [Page(Name = "InternetHerokuapp", Url = "http://{{url}}/")]
    public class InternetHerokuapp : Page
    {
        [Element(Name = "Add/Remove Elements", Locator = "//*[@id=\"content\"]/ul/li[2]/a")]
        A addRemoveElements;

        [Element(Name = "Basic Auth", Locator = "//*[@id=\"content\"]/ul/li[3]/a")]
        A basicAuth;

        [Element(Name = "Checkboxes", Locator = "//*[@id=\"content\"]/ul/li[6]/a")]
        A checkboxes;

        [Element(Name = "Dropdown", Locator = "//*[@id=\"content\"]/ul/li[11]/a")]
        A dropdown;

        [Element(Name = "Dynamic Content", Locator = "//*[@id=\"content\"]/ul/li[12]/a")]
        A dynamicContent;
        
        [Element(Name = "File Download", Locator = "//*[@id=\"content\"]/ul/li[17]/a")]
        A fileDownload;
        
        [Element(Name = "File Upload", Locator = "//*[@id=\"content\"]/ul/li[18]/a")]
        A fileUpload;
        
        [Element(Name = "Frames", Locator = "//*[@id=\"content\"]/ul/li[22]/a")]
        A frames;

        [Element(Name = "Inputs", Locator = "//*[@id=\"content\"]/ul/li[27]/a")]
        A inputs;
    }
}
