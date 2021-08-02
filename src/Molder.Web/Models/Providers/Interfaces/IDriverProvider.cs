using OpenQA.Selenium;
using System;
using System.Collections.ObjectModel;

namespace Molder.Web.Models.Providers
{
    public interface IDriverProvider
    {
        string PageSource { get; }
        string Title { get; }
        string Url { get;  }
        int Tabs { get; }
        string CurrentWindowHandle { get; }
        ReadOnlyCollection<string> WindowHandles { get; }

        void CreateDriver(Func<IWebDriver> action);
        IWebDriver GetDriver();

        void Close();
        void Quit();
        IElementProvider GetElement(By by);
        ReadOnlyCollection<IElementProvider> GetElements(By by);
        void WindowSize(int width, int height);
        void Maximize();
        void Back();
        void Forward();
        void GoToUrl(string url);
        void Refresh();

        void SwitchTo(int number);

        IAlertProvider GetAlert();

        IDriverProvider GetDefaultFrame();
        IDriverProvider GetParentFrame();
        IDriverProvider GetFrame(int id);
        IDriverProvider GetFrame(string name);
        IDriverProvider GetFrame(By by);

        byte[] Screenshot();
    }
}
