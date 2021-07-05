using Molder.Web.Models.PageObjects.Attributes;
using Molder.Web.Models.PageObjects.Pages;
using PageObject.Elements.Frames;

namespace PageObject.Pages.SubPages
{
    [Page(Name = "Nested Frames", Url = "http://{{url}}/nested_frames")]
    public class NestedFrames : Page
    {
        [Frame(Name = "Top", FrameName = "frame-top")]
        TopBottom top;

        [Frame(Name = "Bottom", FrameName = "frame-bottom")]
        Bottom bottom;
    }
}