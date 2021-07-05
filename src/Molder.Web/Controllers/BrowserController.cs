using Molder.Controllers;
using Molder.Helpers;
using Molder.Web.Models.Settings;
using Microsoft.Extensions.Logging;
using System;
using Molder.Web.Models.Browser;
using System.Threading;
using Molder.Web.Infrastructures;

namespace Molder.Web.Controllers
{
    public class BrowserController
    {
        private static AsyncLocal<IBrowser> Browser = new AsyncLocal<IBrowser> { Value = null };
        private static Lazy<VariableController> _variableController;
        private BrowserController() { }
        public static IBrowser GetBrowser()
        {
            return Browser.Value ?? Create();
        }

        private static IBrowser Create()
        {
            if (Browser.Value != null) return Browser.Value;
            switch (BrowserSettings.Settings.Browser)
            {
                case BrowserType.CHROME:
                {
                    Browser.Value = new Chrome();
                    Log.Logger().LogInformation($"ChromeBrowser session is - {Browser.Value.SessionId.ToString()}");
                    return Browser.Value;
                }
                case BrowserType.FIREFOX:
                    break;
                case BrowserType.EDGE:
                    break;
                case BrowserType.OPERA:
                    break;
                default:
                    throw new InvalidOperationException("unknown browser type");
            }
            return Browser.Value;
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

        public static void SetVariables(VariableController variableController) => _variableController = new Lazy<VariableController>(() => variableController);
    }
}
