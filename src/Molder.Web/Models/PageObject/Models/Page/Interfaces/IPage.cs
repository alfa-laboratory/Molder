using Molder.Web.Models.PageObject.Models.Blocks;
using Molder.Web.Models.PageObject.Models.Elements.Interfaces;
using System.Collections.Generic;

namespace Molder.Web.Models.PageObject.Models.Page.Interfaces
{
    public interface IPage
    {
        string Name { get; set; }
        string Url { get; set; }

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
