using Molder.Web.Extensions;
using Molder.Web.Infrastructures;
using Molder.Web.Models.PageObjects.Elements;
using Molder.Web.Models.PageObjects.Frames;

namespace Molder.Web.Models.PageObjects.Blocks
{
    public class Block : Element
    {
        public Block(string name, string locator, bool optional) : base(name, locator, optional) {  }

        public Block GetBlock(string name)
        {
            var block = Root.SearchElementBy(name, ObjectType.Block);

            (block.Object as Block)?.SetProvider(_driverProvider);
            ((Block) block.Object).Root = block;
            return (Block) block.Object;
        }

        public new IElement GetElement(string name)
        {
            var element = Root.SearchElementBy(name);
            (element.Object as Element)?.SetProvider(_driverProvider);
            ((Element) element.Object).Root = element;
            return (IElement) element.Object;
        }

        public Frame GetFrame(string name)
        {
            var frame = Root.SearchElementBy(name, ObjectType.Frame);

            (frame.Object as Frame)?.SetProvider(_driverProvider);
            ((Frame) frame.Object).Root = frame;
            return (Frame) frame.Object;
        }
    }
}