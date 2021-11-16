using System;
using Microsoft.Extensions.Logging;
using Molder.Helpers;
using Molder.Web.Extensions;
using Molder.Web.Infrastructures;
using Molder.Web.Models.Settings;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace Molder.Web.Models.Browser
{
    public class Firefox : Browser
    {
        public sealed override SessionId SessionId { get; protected set; }
        
        public Firefox()
        {
            var options = CreateOptions();
            if(BrowserSettings.Settings.IsRemoteRun())
            {
                Log.Logger().LogInformation($@"Start remote firefox browser...");
                DriverProvider.CreateDriver(() => new RemoteWebDriver(new Uri(BrowserSettings.Settings.Remote.Url), options.ToCapabilities()));
                SessionId = (DriverProvider.GetDriver() as RemoteWebDriver)?.SessionId;
                Log.Logger().LogInformation($@"Remote firefox browser (SessionId is {SessionId}) is starting with options: {Helpers.Message.CreateMessage(options)}");
                return;
            }
            Log.Logger().LogInformation($@"Start firefox browser...");
            var service = FirefoxDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            DriverProvider.CreateDriver(() => new FirefoxDriver(service, options));
            SessionId = (DriverProvider.GetDriver() as FirefoxDriver)?.SessionId;
            Log.Logger().LogInformation($@"Local firefox browser (SessionId is {SessionId}) is starting with options: {Helpers.Message.CreateMessage(options)}");
        }

        private FirefoxOptions CreateOptions()
        {
            var options = new FirefoxOptions();
            
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
                options.BrowserExecutableLocation = BrowserSettings.Settings.BinaryLocation;
            }

            if (BrowserSettings.Settings.CheckCapability())
            {
                options.AddCapabilities(BrowserSettings.Settings.Capabilities);
            }
            
            return options;
        }
    }
}