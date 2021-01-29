using Molder.Web.Models.PageObjects.Alerts;
using Molder.Web.Models.PageObjects.Pages;
using OpenQA.Selenium.Remote;

namespace Molder.Web.Models.Browser
{
    public interface IBrowser
    {
        string Url { get; }
        string Title { get; }
        SessionId SessionId { get; }
        int Tabs { get; }

        bool Close();
        bool Quit();
        bool WindowSize(int width, int height);
        void Maximize();
        void Back();
        void Forward();
        bool GoToPage(string url);
        bool Refresh();

        void SetCurrentPage(string name, bool loading = true);
        void UpdateCurrentPage(string name);
        IPage GetCurrentPage();

        void SwitchTo(int number);
        IAlert Alert();
    }
}
