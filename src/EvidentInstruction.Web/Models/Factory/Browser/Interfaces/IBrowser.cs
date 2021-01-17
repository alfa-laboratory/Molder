using EvidentInstruction.Web.Models.PageObject.Models.Alert.Interfaces;
using EvidentInstruction.Web.Models.PageObject.Models.Page.Interfaces;
using OpenQA.Selenium.Remote;

namespace EvidentInstruction.Web.Models.Factory.Browser.Interfaces
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
