using EvidentInstruction.Web.Models.PageObject.Attributes;
using EvidentInstruction.Web.Models.PageObject.Models.Blocks;
using PageObject.Elements.Elements;

namespace PageObject.Elements.Blocks
{
    public class BlockC : Block
    {
        [Element(Name = "inputBlockC", Locator = "inputBlockC")]
        Input inputBlockC;

        [Element(Name = "buttonBlockC", Locator = "buttonBlockC")]
        Button buttonBlockC;

        public BlockC(string name, string locator, string block = null) : base(name, locator, block)
        {
        }
    }
}
