using System;
using System.Collections.Generic;
using System.Text;

namespace EvidentInstruction.Web.Models.PageObject.Models.Interfaces
{
    public interface IPage
    {
        string Name { get; }
        string Url { get; }
        string BrowserUrl { get; }
        string Title { get; }

        IElement GetElement(string name);
        IEnumerable<IElement> GetPrimaryElements();
        IBlock GetBlock(string name);

        void GoToPage();

        void Refresh();
        void Maximize();
        void SetWindowSize(int width, int height);
        void PageTop();
        void PageDown();
        void Close();

        bool IsAppeared();
        bool IsDisappeared();
        void IsPageLoad();
    }
}
