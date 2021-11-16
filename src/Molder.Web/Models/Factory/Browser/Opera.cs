using System;
using Microsoft.Extensions.Logging;
using Molder.Helpers;
using Molder.Web.Extensions;
using Molder.Web.Infrastructures;
using Molder.Web.Models.Settings;
using OpenQA.Selenium;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.Remote;

namespace Molder.Web.Models.Browser
{
    public class Opera : Browser
    {
        public sealed override SessionId SessionId { get; protected set; }
        
        public Opera()
        {
            var options = CreateOptions();
            if(BrowserSettings.Settings.IsRemoteRun())
            {
                Log.Logger().LogInformation($@"Start remote opera browser...");
                DriverProvider.CreateDriver(() => new RemoteWebDriver(new Uri(BrowserSettings.Settings.Remote.Url), options.ToCapabilities()));
                SessionId = (DriverProvider.GetDriver() as RemoteWebDriver)?.SessionId;
                Log.Logger().LogInformation($@"Remote opera browser (SessionId is {SessionId}) is starting with options: {Helpers.Message.CreateMessage(options)}");
                return;
            }
            Log.Logger().LogInformation($@"Start opera browser...");
            var service = OperaDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            DriverProvider.CreateDriver(() => new OperaDriver(service, options));
            SessionId = (DriverProvider.GetDriver() as OperaDriver)?.SessionId;
            Log.Logger().LogInformation($@"Local opera browser (SessionId is {SessionId}) is starting with options: {Helpers.Message.CreateMessage(options)}");
        }

        private OperaOptions CreateOptions()
        {
            var options = new OperaOptions();
            
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