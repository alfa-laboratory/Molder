using EvidentInstruction.Web.Models.PageObject.Attributes;
using EvidentInstruction.Web.Models.PageObject.Models.Blocks;
using PageObject.Elements.Elements;

namespace PageObject.Elements.Blocks
{
    public class BlockBB : Block
    {
        [Block(Name = "Блок БББ", Locator = "blockBBB xpath", Optional = true)]
        BlockBBB blockBBB;

        [Element(Name = "checkboxBlockBBB1", Locator = "checkboxBlockBBB1 xpath", Optional = true)]
        Checkbox checkboxBlockBBB1;
        [Element(Name = "checkboxBlockBBB1", Locator = "checkboxBlockBBB1 xpath")]
        Checkbox checkboxBlockBBB2;
        [Element(Name = "checkboxBlockBBB1", Locator = "checkboxBlockBBB1 xpath", Optional = true)]
        Checkbox checkboxBlockBBB3;

        public BlockBB(string name, string locator, string block = null) : base(name, locator, block)
        {
        }
    }
}
