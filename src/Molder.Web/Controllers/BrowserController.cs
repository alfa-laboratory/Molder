using Molder.Controllers;
using Molder.Helpers;
using Molder.Web.Models.Settings;
using Microsoft.Extensions.Logging;
using System;
using Molder.Web.Models.Browser;
using Molder.Web.Models.Proxy;
using System.Threading;

namespace Molder.Web.Controllers
{
    public class BrowserController
    {
        private static AsyncLocal<IBrowser> _browser = new AsyncLocal<IBrowser> { Value = null };
        private static AsyncLocal<VariableController> _variables = new AsyncLocal<VariableController> { Value = null };
        private static AsyncLocal<Authentication> _authentication = new AsyncLocal<Authentication> { Value = null };

        private BrowserController() { }

        public static IBrowser GetBrowser()
        {
            if (_browser.Value == null)
            {
                var settings = new BrowserSetting(_variables.Value);
                settings.Create();

                return Create(settings);
            }
            return _browser.Value;
        }

        public static IBrowser Create(ISetting setting)
        {
            var browserSetting = setting as BrowserSetting;
            browserSetting.Authentication = _authentication.Value;
            if (_browser.Value == null)
            {
                switch (browserSetting.BrowserType)
                {
                    case Infrastructures.BrowserType.CHROME:
                        {
                            _browser.Value = new Chrome(browserSetting);
                            Log.Logger().LogInformation($"Сессия браузера - {_browser.Value.SessionId.ToString()}");
                            return _browser.Value;
                        }
                    default:
                        throw new InvalidOperationException("unknown browser type");
                }
            }
            return _browser.Value;
        }

        public static void Quit()
        {
            if (_browser.Value != null)
            {
                _browser.Value.Dispose();
                _browser.Value.Quit();
                _browser.Value = null;
            }
        }

        public static void SetVariables(VariableController variables)
        {
            _variables.Value = variables;
        }

        public static void CreateProxy(Authentication authentification)
        {
            _authentication.Value = authentification;
        }
    }
}
