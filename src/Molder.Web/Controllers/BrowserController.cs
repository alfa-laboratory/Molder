using Molder.Helpers;
using Molder.Web.Models.Settings;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using Molder.Web.Models.Browser;
using System.Threading;
using Molder.Web.Infrastructures;

namespace Molder.Web.Controllers
{
    [ExcludeFromCodeCoverage]
    public class BrowserController
    {
        private static AsyncLocal<IBrowser> Browser = new() { Value = null };
        private BrowserController() { }
        public static IBrowser GetBrowser()
        {
            return Browser.Value ?? Create();
        }

        private static IBrowser Create()
        {
            switch (BrowserSettings.Settings.Browser)
            {
                case BrowserType.CHROME:
                {
                    Browser.Value = new Chrome();
                    Log.Logger().LogInformation($"ChromeBrowser session is - {Browser.Value.SessionId}");
                    return Browser.Value;
                }
                case BrowserType.FIREFOX:
                {
                    Browser.Value = new Firefox();
                    Log.Logger().LogInformation($"FirefoxBrowser session is - {Browser.Value.SessionId}");
                    return Browser.Value;
                }
                case BrowserType.EDGE:
                {
                    Browser.Value = new Edge();
                    Log.Logger().LogInformation($"EdgeBrowser session is - {Browser.Value.SessionId}");
                    return Browser.Value;
                }
                case BrowserType.OPERA:
                {
                    Browser.Value = new Opera();
                    Log.Logger().LogInformation($"OperaBrowser session is - {Browser.Value.SessionId}");
                    return Browser.Value;
                }
                default:
                    throw new InvalidOperationException($"unknown browser type {BrowserSettings.Settings.Browser.ToString()}");
            }
        }

        public static void Quit()
        {
            if (GetBrowser() == null) return;
            GetBrowser().Dispose();
            Log.Logger().LogInformation("Browser is disposed");
            GetBrowser().Quit();
            Log.Logger().LogInformation("Browser is quited");
            Browser.Value = null;
        }
    }
}