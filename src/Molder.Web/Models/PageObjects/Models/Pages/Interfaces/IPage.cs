using Molder.Web.Models.PageObjects.Blocks;
using Molder.Web.Models.PageObjects.Elements;
using Molder.Web.Models.PageObjects.Frames;
using System.Collections.Generic;

namespace Molder.Web.Models.PageObjects.Pages
{
    public interface IPage
    {
        string Name { get; }
        string Url { get; }
        Node Root { get; set; }
        Node Local { get; set; }

        Block GetBlock(string name);
        void BackToPage();
        IElement GetElement(string name);
        IEnumerable<string> GetPrimaryElements();

        IPage GetDefaultFrame();
        Frame GetParentFrame();
        Frame GetFrame(string name);

        void GoToPage();
        void PageTop();
        void PageDown();

        bool IsLoadElements();
    }
}