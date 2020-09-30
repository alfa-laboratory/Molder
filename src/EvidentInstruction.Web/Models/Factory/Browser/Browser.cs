using EvidentInstruction.Web.Models.Factory.Browser.Interfaces;
using EvidentInstruction.Web.Models.Providers;
using EvidentInstruction.Web.Models.Providers.Interfaces;
using OpenQA.Selenium;
using System.Collections.ObjectModel;

namespace EvidentInstruction.Web.Models.Factory.Browser
{
    public abstract class Browser : IBrowser
    {
        public IDriverProvider _provider = new DriverProvider();

        public bool Close()
        {
            return _provider.Close();
        }

        public bool Quit()
        {
            return _provider.Quit();
        }

        public IElementProvider GetElement(By by)
        {
            return _provider.GetElement(by);
        }

        public ReadOnlyCollection<IElementProvider> GetElements(By by)
        {
            return _provider.GetElements(by);
        }

        public IElementProvider GetActiveElement()
        {
            return _provider.GetActiveElement();
        }

        public bool WindowSize(int width, int height)
        {
            return _provider.WindowSize(width, height);
        }

        public void Maximize()
        {
            _provider.Maximize();
        }

        public void Back()
        {
            _provider.Back();
        }

        public void Forward()
        {
            _provider.Forward();
        }

        public bool GoToUrl(string url)
        {
            return _provider.GoToUrl(url);
        }

        public bool GoToUrl(System.Uri url)
        {
            return _provider.GoToUrl(url);
        }

        public bool Refresh()
        {
            return _provider.Refresh();
        }

        public IAlertProvider GetAlert()
        {
            return _provider.GetAlert();
        }

        public IDriverProvider GetDefaultFrame()
        {
            return _provider.GetDefaultFrame();
        }

        public IDriverProvider GetFrame(int id)
        {
            return _provider.GetFrame(id);
        }

        public IDriverProvider GetFrame(string name)
        {
            return _provider.GetFrame(name);
        }
        public IDriverProvider GetFrame(By by)
        {
            return _provider.GetFrame(by);
        }

        public Screenshot Screenshot()
        {
            return _provider.Screenshot();
        }
    }
}
