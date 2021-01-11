using EvidentInstruction.Web.Models.PageObject.Models.Blocks;
using EvidentInstruction.Web.Models.PageObject.Models.Elements.Interfaces;
using System.Collections.Generic;

namespace EvidentInstruction.Web.Models.PageObject.Models.Page.Interfaces
{
    public interface IPage
    {
        Block GetBlock(string name);
        IElement GetElement(string name);
        IEnumerable<IElement> GetPrimaryElements();

        bool GoToPage();
        void PageTop();
        void PageDown();

        bool IsLoad();
    }
}
