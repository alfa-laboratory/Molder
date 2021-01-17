using EvidentInstruction.Web.Models.PageObject.Attributes;
using EvidentInstruction.Web.Models.PageObject.Models.Blocks;
using EvidentInstruction.Web.Models.PageObject.Models.Elements;

namespace PageObject.Elements.Blocks
{
    public class FrsContent : Block
    {
        public FrsContent(string name, string locator, bool optional) : base(name, locator, optional) {   }

        [Element(Name = "Text", Locator = "//*[@id=\"content\"]/div[1]/div[2]")]
        Div text;
    }
}
