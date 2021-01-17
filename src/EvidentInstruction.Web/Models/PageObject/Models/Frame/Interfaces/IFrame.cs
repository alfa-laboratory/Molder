using EvidentInstruction.Web.Models.PageObject.Models.Blocks;
using EvidentInstruction.Web.Models.PageObject.Models.Elements.Interfaces;
using EvidentInstruction.Web.Models.Providers.Interfaces;
using System.Collections.Concurrent;

namespace EvidentInstruction.Web.Models.PageObject.Models.Interfaces
{
    public interface IFrame
    {
        IDriverProvider Get();
        IDriverProvider Parent();
        IDriverProvider Default();

        Block GetBlock(string name);
        ConcurrentDictionary<string, Block> GetBlocks();
        IElement GetElement(string name);
        IFrame GetFrame(string name);
    }
}
