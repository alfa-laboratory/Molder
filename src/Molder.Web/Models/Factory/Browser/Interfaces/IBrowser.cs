using Molder.Web.Models.PageObjects.Pages;
using OpenQA.Selenium;
using IAlert = Molder.Web.Models.PageObjects.Alerts.IAlert;

namespace Molder.Web.Models.Browser
{
    public interface IBrowser
    {
        string Url { get; }
        string Title { get; }
        SessionId SessionId { get; }
        int Tabs { get; }

        void Close();
        void Quit();
        void WindowSize(int width, int height);
        void Maximize();
        void Back();
        void Forward();
        void GoToPage(string url);
        void Refresh();

        void SetCurrentPage(string name, bool loading = true);
        void UpdateCurrentPage(string name);
        IPage GetCurrentPage();

        void SwitchTo(int number);
        IAlert Alert();

        void Dispose();
    }
}