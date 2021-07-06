using Microsoft.Extensions.Logging;
using Molder.Helpers;
using Molder.Web.Extensions;
using Molder.Web.Infrastructures;
using Molder.Web.Models.Settings;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;

namespace Molder.Web.Models.Browser
{
    public class Chrome : Browser
    {
        public sealed override SessionId SessionId { get; protected set; }

        public Chrome()
        {
            var options = CreateOptions();
            if(BrowserSettings.Settings.IsRemoteRun())
            {
                WebDriver.CreateDriver(() => new RemoteWebDriver(new Uri(BrowserSettings.Settings.Remote.Url), options.ToCapabilities()));
                SessionId = (WebDriver.GetDriver() as RemoteWebDriver)?.SessionId;
                Log.Logger().LogInformation($@"Remote chrome browser (SessionId is {SessionId}) is starting with options: {Helpers.Message.CreateMessage(options)}");
                return;
            }
            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            WebDriver.CreateDriver(() => new ChromeDriver(service, options));
            SessionId = (WebDriver.GetDriver() as ChromeDriver)?.SessionId;
            Log.Logger().LogInformation($@"Local chrome browser (SessionId is {SessionId}) is starting with options: {Helpers.Message.CreateMessage(options)}");
        }

        private ChromeOptions CreateOptions()
        {
            var options = new ChromeOptions();
            
            if (BrowserSettings.Settings.IsRemoteRun())
            {
                options.AddAdditionalCapability("version", BrowserSettings.Settings.Remote.Version ?? Constants.DEFAULT_VERSION, true);
                options.AddAdditionalCapability("enableVNC", true, true);
                options.AddAdditionalCapability("platform", BrowserSettings.Settings.Remote.Platform ?? Constants.DEFAULT_PLATFORM, true);
                options.AddAdditionalCapability("name", BrowserSettings.Settings.Remote.Project ?? Constants.DEFAULT_PROJECT, true);
            }

            if (BrowserSettings.Settings.IsOptions())
            {
                options.AddArguments(BrowserSettings.Settings.Options);
            }
            
            if(!BrowserSettings.Settings.IsBinaryPath())
            {
                options.BinaryLocation = BrowserSettings.Settings.BinaryLocation;
            }

            if (BrowserSettings.Settings.CheckCapability())
            {
                options.AddCapabilities(BrowserSettings.Settings.Capabilities);
            }
            
            return options;
        }
    }
}