using Molder.Web.Models.PageObjects.Blocks;
using Molder.Web.Models.PageObjects.Elements;
using Molder.Web.Models.PageObjects.Frames;
using System.Collections.Generic;

namespace Molder.Web.Models.PageObjects.Pages
{
    public interface IPage
    {
        string Name { get; set; }
        string Url { get; set; }
        Node Root { get; set; }

        Block GetBlock(string name);
        IElement GetElement(string name);
        IEnumerable<IElement> GetPrimaryElements();

        IPage GetDefaultFrame();
        Frame GetParentFrame();
        Frame GetFrame(string name);

        bool GoToPage();
        void PageTop();
        void PageDown();

        bool IsLoadElements();
    }
}