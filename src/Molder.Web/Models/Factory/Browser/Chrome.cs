using Molder.Web.Extensions;
using Molder.Web.Models.Settings;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;

namespace Molder.Web.Models.Browser
{
    public class Chrome : Browser
    {
        public override SessionId SessionId { get; protected set; }

        public Chrome(ISetting setting = null)
        {
            _proxyServer.Value = new Proxy.Proxy();
            Settings = setting as BrowserSetting;
            var options = GetOptions(Settings);

            if (((BrowserSetting)Settings)?.Remote == true)
            {
                var isRemoteRunning = setting.IsRemoteRunning();
                if (isRemoteRunning)
                {
                    _provider.Value.CreateDriver(() => new RemoteWebDriver(new Uri(((BrowserSetting)Settings).RemoteUrl), options.ToCapabilities()), Settings);
                    SessionId = (_provider.Value.GetDriver() as RemoteWebDriver).SessionId;
                    return;
                }
            }

            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            if (((BrowserSetting)Settings).BrowserPath != null)
            {
                _provider.Value.CreateDriver(() => new ChromeDriver(((BrowserSetting)Settings).BrowserPath, options), Settings);
                SessionId = (_provider.Value.GetDriver() as ChromeDriver).SessionId;
                return;
            }
            _provider.Value.CreateDriver(() => new ChromeDriver(service, options), Settings);
            SessionId = (_provider.Value.GetDriver() as ChromeDriver).SessionId;
        }

        protected ChromeOptions GetOptions(ISetting setting)
        {
            var options = new ChromeOptions();

            var browserSetting = setting as BrowserSetting;
            
            if(browserSetting?.Remote == true)
            {
                options.AddAdditionalCapability("version", browserSetting.RemoteVersion, true);
                options.AddAdditionalCapability("enableVNC", true, true);
                options.AddAdditionalCapability("platform", "ANY", true);
            }
            options.AddArguments("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            if (browserSetting.Headless == true)
            {
                options.AddArguments("--headless");
            }
            options.AddArguments("--disable-gpu");

            if (browserSetting.Authentication != null)
            {
                var proxy = new OpenQA.Selenium.Proxy();
                int localPort = _proxyServer.Value.AddEndpoint(browserSetting.Authentication);
                proxy.HttpProxy = $"127.0.0.1:{localPort}";
                options.Proxy = proxy;
                options.AddArgument($"--proxy-server=127.0.0.1:{localPort}");
            }
            return options;
        }
    }
}
