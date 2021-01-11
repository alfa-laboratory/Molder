using EvidentInstruction.Web.Models.PageObject.Attributes;
using EvidentInstruction.Web.Models.PageObject.Models.Blocks;
using PageObject.Elements.Elements;

namespace PageObject.Elements.Blocks
{
    public class BlockA : Block
    {
        [Block(Name = "Блок АА", Locator = "blockAA Xpath")]
        BlockAA blockAA;

        [Element(Name = "blockATable", Locator = "blockATable xpath", Optional = true)]
        Table blockATable;

        [Element(Name = "checkboxBlockA", Locator = "checkboxBlockA xpath")]
        Checkbox checkboxBlockA;

        public BlockA(string name, string locator, string block = null) : base(name, locator, block)
        {
        }
    }
}
