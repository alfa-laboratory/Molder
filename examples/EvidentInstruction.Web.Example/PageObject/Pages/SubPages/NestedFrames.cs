using EvidentInstruction.Web.Models.PageObject.Attributes;
using EvidentInstruction.Web.Models.PageObject.Models;
using EvidentInstruction.Web.Models.PageObject.Models.Page;
using PageObject.Elements.Frames;

namespace PageObject.Pages.SubPages
{
    [Page(Name = "Nested Frames", Url = "http://192.168.99.100:7080/nested_frames")]
    public class NestedFrames : Page
    {
        [Frame(Name = "Left", FrameName = "frame-left")]
        LeftBottom left;

        [Frame(Name = "Bottom", FrameName = "frame-bottom")]
        LeftBottom bottom;
    }
}