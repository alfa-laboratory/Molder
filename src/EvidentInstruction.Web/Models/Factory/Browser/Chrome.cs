using EvidentInstruction.Web.Models.Settings;
using EvidentInstruction.Web.Models.Settings.Interfaces;
using OpenQA.Selenium.Chrome;

namespace EvidentInstruction.Web.Models.Factory.Browser
{
    public class Chrome : Browser
    {
        public Chrome(ISetting setting = null)
        {
            var browserSetting = setting as BrowserSetting;

            var options = GetOptions(setting);
            if (browserSetting.BrowserPath != null)
            {
                var pathDriver = new ChromeDriver(browserSetting.BrowserPath, options);
                _provider.SetDriver(pathDriver);
            }
            var driver = new ChromeDriver(options);
            _provider.SetDriver(driver);
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
