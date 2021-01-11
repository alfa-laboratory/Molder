using EvidentInstruction.Web.Models.Providers;
using EvidentInstruction.Web.Models.Settings;
using EvidentInstruction.Web.Models.Settings.Interfaces;
using OpenQA.Selenium.Chrome;

namespace EvidentInstruction.Web.Models.Factory.Browser
{
    public class Chrome : Browser
    {
        public Chrome(ISetting setting = null)
        {
            if (setting == null)
            {
                Settings = new BrowserSetting();
                Settings.Create();
            }
            else
            {
                Settings = setting as BrowserSetting;
            }

            var options = GetOptions(Settings);
            if (((BrowserSetting)Settings).BrowserPath != null)
            {
                _provider.CreateDriver(() => new ChromeDriver(((BrowserSetting)Settings).BrowserPath, options), Settings);
            }
            _provider.CreateDriver(() => new ChromeDriver(options), Settings);
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
