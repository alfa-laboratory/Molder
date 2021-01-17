using EvidentInstruction.Web.Extensions;
using EvidentInstruction.Web.Models.Settings;
using EvidentInstruction.Web.Models.Settings.Interfaces;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;

namespace EvidentInstruction.Web.Models.Factory.Browser
{
    public class Chrome : Browser
    {
        public override SessionId SessionId { get; protected set; }

        public Chrome(ISetting setting = null)
        {
            Settings = setting as BrowserSetting;
            var options = GetOptions(Settings);

            if (((BrowserSetting)Settings)?.Remote == true)
            {
                var isRemoteRunning = setting.IsRemoteRunning();
                if (isRemoteRunning)
                {
                    _provider.CreateDriver(() => new RemoteWebDriver(new Uri(((BrowserSetting)Settings).RemoteUrl), options.ToCapabilities()), Settings);
                    SessionId = (_provider.GetDriver() as RemoteWebDriver).SessionId;
                    return;
                }
            }

            if (((BrowserSetting)Settings).BrowserPath != null)
            {
                _provider.CreateDriver(() => new ChromeDriver(((BrowserSetting)Settings).BrowserPath, options), Settings);
                SessionId = (_provider.GetDriver() as ChromeDriver).SessionId;
                return;
            }
            _provider.CreateDriver(() => new ChromeDriver(options), Settings);
            SessionId = (_provider.GetDriver() as ChromeDriver).SessionId;
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
            if (browserSetting.Headless == true)
            {
                options.AddArguments("--headless");
            }
            options.AddArguments("disable-gpu");

            return options;
        }
    }
}
