using EvidentInstruction.Web.Models.PageObject.Models.Blocks;
using EvidentInstruction.Web.Models.PageObject.Models.Elements.Interfaces;
using EvidentInstruction.Web.Models.PageObject.Models.Interfaces;
using System.Collections.Generic;

namespace EvidentInstruction.Web.Models.PageObject.Models.Page.Interfaces
{
    public interface IPage
    {
        string Name { get; set; }
        string Url { get; set; }

        Block GetBlock(string name);
        IElement GetElement(string name);
        IEnumerable<IElement> GetPrimaryElements();

        IPage GetDefaultFrame();
        IFrame GetParentFrame();
        IFrame GetFrame(string name);

        bool GoToPage();
        void PageTop();
        void PageDown();

        bool IsLoadElements();
    }
}
