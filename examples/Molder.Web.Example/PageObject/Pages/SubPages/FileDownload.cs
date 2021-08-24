using Molder.Web.Models.PageObjects.Attributes;
using Molder.Web.Models.PageObjects.Elements;
using Molder.Web.Models.PageObjects.Pages;

namespace PageObject.Pages.SubPages
{
    [Page(Name = "File Download", Url = "http://{{url}}/download")]
    public class FileDownload : Page
    {
        [Element(Name = "Txt File", Locator = "//*[@id=\"content\"]/div/a")]
        A file;
    }
}