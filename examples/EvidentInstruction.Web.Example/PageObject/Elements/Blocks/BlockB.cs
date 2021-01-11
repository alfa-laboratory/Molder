using EvidentInstruction.Web.Models.PageObject.Attributes;
using EvidentInstruction.Web.Models.PageObject.Models.Blocks;

namespace PageObject.Elements.Blocks
{
    public class BlockB : Block
    {
        [Block(Name = "Блок ББ", Locator = "blockBB xpath")]
        BlockBB blockBB;

        public BlockB(string name, string locator, string block = null) : base(name, locator, block)
        {
        }
    }
}
