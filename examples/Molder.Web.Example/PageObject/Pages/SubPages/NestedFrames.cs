using Molder.Web.Models.PageObjects.Attributes;
using Molder.Web.Models.PageObjects.Pages;
using PageObject.Elements.Frames;

namespace PageObject.Pages.SubPages
{
    [Page(Name = "Nested Frames", Url = "http://192.168.99.100:9080/nested_frames")]
    public class NestedFrames : Page
    {
        [Frame(Name = "Left", FrameName = "frame-left")]
        LeftBottom left;

        [Frame(Name = "Bottom", FrameName = "frame-bottom")]
        LeftBottom bottom;
    }
}