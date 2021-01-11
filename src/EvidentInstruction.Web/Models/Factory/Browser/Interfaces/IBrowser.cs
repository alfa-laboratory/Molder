using EvidentInstruction.Web.Models.PageObject.Models.Page.Interfaces;
using System;

namespace EvidentInstruction.Web.Models.Factory.Browser.Interfaces
{
    public interface IBrowser
    {
        string Url { get; }
        string Title { get; }

        bool Close();
        bool Quit();
        bool WindowSize(int width, int height);
        void Maximize();
        void Back();
        void Forward();
        bool GoToPage(string url);
        bool Refresh();

        void SetCurrentPage(string name);
        IPage GetCurrentPage();
    }
}
