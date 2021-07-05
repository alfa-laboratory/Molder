using Molder.Web.Models.PageObjects.Attributes;
using Molder.Web.Models.PageObjects.Elements;
using Molder.Web.Models.PageObjects.Frames;

namespace PageObject.Elements.Frames
{
    public class Bottom : Frame
    {
        public Bottom(string name, string frameName, int? number, string locator, bool optional = false) : base(name, frameName, number, locator, optional) { }

        [Element(Name = "Text", Locator = "/html/body")]
        Div text;
    }
}
