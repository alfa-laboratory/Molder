using System;
using Microsoft.Extensions.Logging;
using Molder.Helpers;
using Molder.Web.Extensions;
using Molder.Web.Models.Settings;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Remote;

namespace Molder.Web.Models.Browser
{
    public class Edge : Browser
    {
        public sealed override SessionId SessionId { get; protected set; }
        
        public Edge()
        {
            var options = CreateOptions();
            if(BrowserSettings.Settings.IsRemoteRun())
            {
                Log.Logger().LogInformation($@"Start remote edge browser...");
                DriverProvider.CreateDriver(() => new RemoteWebDriver(new Uri(BrowserSettings.Settings.Remote.Url), options.ToCapabilities()));
                SessionId = (DriverProvider.GetDriver() as RemoteWebDriver)?.SessionId;
                Log.Logger().LogInformation($@"Remote edge browser (SessionId is {SessionId}) is starting with options: {Helpers.Message.CreateMessage(options)}");
                return;
            }
            Log.Logger().LogInformation($@"Start edge browser...");
            var service = EdgeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            DriverProvider.CreateDriver(() => new EdgeDriver(service, options));
            SessionId = (DriverProvider.GetDriver() as EdgeDriver)?.SessionId;
            Log.Logger().LogInformation($@"Local edge browser (SessionId is {SessionId}) is starting with options: {Helpers.Message.CreateMessage(options)}");
        }

        private EdgeOptions CreateOptions()
        {
            var options = new EdgeOptions();

            if (BrowserSettings.Settings.CheckCapability())
            {
                options.AddCapabilities(BrowserSettings.Settings.Capabilities);
            }
            
            return options;
        }
    }
}