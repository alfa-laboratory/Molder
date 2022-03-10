using Molder.Web.Models.PageObjects.Attributes;
using Molder.Web.Models.PageObjects.Frames;

namespace PageObject.Elements.Frames
{
    public class TopBottom : Frame
    {
        public TopBottom(string name, string frameName, int? number, string locator, bool optional = false) : base(name, frameName, number, locator, optional) { }

        [Frame(Name = "Left", FrameName = "frame-left")]
        LeftBottom left;
    }
}
