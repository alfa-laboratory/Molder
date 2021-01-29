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

            (block.Object as Block).SetProvider(_driverProvider);
            (block.Object as Block).Root = block;
            return block.Object as Block;
        }

        public IElement GetElement(string name)
        {
            var element = Root.SearchElementBy(name);
            (element.Object as Element).SetProvider(_driverProvider);
            (element.Object as Element).Root = element;
            return element.Object as IElement;
        }

        public Frame GetFrame(string name)
        {
            var frame = Root.SearchElementBy(name, ObjectType.Frame);

            (frame.Object as Frame).SetProvider(_driverProvider);
            (frame.Object as Frame).Root = frame;
            return frame.Object as Frame;
        }
    }
}
