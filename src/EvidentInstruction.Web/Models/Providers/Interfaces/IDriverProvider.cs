using OpenQA.Selenium;
using System.Collections.ObjectModel;

namespace EvidentInstruction.Web.Models.Providers.Interfaces
{
    public interface IDriverProvider
    {
        string PageSource { get;}
        string Title { get; }
        string Url { get;  }
        string CurrentWindowHandle { get; }
        ReadOnlyCollection<string> WindowHandles { get; }

        void SetDriver(IWebDriver driver);

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
        bool GoToUrl(System.Uri url);
        bool Refresh();

        IAlertProvider GetAlert();

        IDriverProvider GetDefaultFrame();
        IDriverProvider GetFrame(int id);
        IDriverProvider GetFrame(string name);
        IDriverProvider GetFrame(By by);

        Screenshot Screenshot();
    }
}
