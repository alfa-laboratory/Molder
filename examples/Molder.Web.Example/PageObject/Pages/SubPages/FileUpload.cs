using Molder.Web.Models.PageObjects.Attributes;
using Molder.Web.Models.PageObjects.Elements;
using Molder.Web.Models.PageObjects.Pages;

namespace PageObject.Pages.SubPages
{
    [Page(Name = "File Upload", Url = "http://{{url}}/download")]
    public class FileUpload : Page
    {
        [Element(Name = "SetPath", Locator = "//*[@id=\"file-upload\"]")]
        File fileUpload;
        
        [Element(Name = "Upload", Locator = "//*[@id=\"file-submit\"]")]
        Button submit;

        [Element(Name = "Header", Locator = "//*[@id=\"content\"]/div/h3", Optional = true)]
        Default header;
        
        [Element(Name = "Files", Locator = "//*[@id=\"uploaded-files\"]/text()", Optional = true)]
        Default files;
    }
}