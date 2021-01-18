using Molder.Web.Models.Settings.Interfaces;
using OpenQA.Selenium;
using System;
using System.Collections.ObjectModel;

namespace Molder.Web.Models.Providers.Interfaces
{
    public interface IDriverProvider
    {
        ISetting Settings { get; set; }
        string PageSource { get; }
        string Title { get; }
        string Url { get;  }
        int Tabs { get; }
        string CurrentWindowHandle { get; }
        ReadOnlyCollection<string> WindowHandles { get; }

        void CreateDriver(Func<IWebDriver> action, ISetting settings);
        IWebDriver GetDriver();

        bool Close();
        bool Quit();
        IElementProvider GetElement(By by);
        ReadOnlyCollection<IElementProvider> GetElements(By by);
        IElementProvider GetActiveElement();
        bool WindowSize(int width, int height);
        void Maximize();
        void Back();
        void Forward();
        bool GoToUrl(string url);
        bool Refresh();

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
