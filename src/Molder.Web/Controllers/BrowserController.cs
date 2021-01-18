using Molder.Controllers;
using Molder.Helpers;
using Molder.Web.Models.Factory.Browser;
using Molder.Web.Models.Factory.Browser.Interfaces;
using Molder.Web.Models.Settings;
using Molder.Web.Models.Settings.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace Molder.Web.Controllers
{
    public class BrowserController
    {
        [ThreadStatic]
        private static IBrowser _browser;
        [ThreadStatic]
        private static VariableController _variables;

        private BrowserController() { }

        public static IBrowser GetBrowser()
        {
            if (_browser == null)
            {
                var settings = new BrowserSetting(_variables);
                settings.Create();

                return Create(settings);
            }
            return _browser;
        }

        public static IBrowser Create(ISetting setting)
        {
            var browserSetting = setting as BrowserSetting;
            if (_browser == null)
            {
                switch (browserSetting.BrowserType)
                {
                    case Infrastructures.BrowserType.CHROME:
                        {
                            _browser = new Chrome(browserSetting);
                            Log.Logger().LogInformation($"Сессия браузера - {_browser.SessionId.ToString()}");
                            return _browser;
                        }
                    default:
                        throw new InvalidOperationException("unknown browser type");
                }
            }
            return _browser;
        }

        public static void Quit()
        {
            if (_browser != null)
            {
                _browser.Quit();
                _browser = null;
            }
        }

        public static void SetVariables(VariableController variables)
        {
            _variables = variables;
        }
    }
}
