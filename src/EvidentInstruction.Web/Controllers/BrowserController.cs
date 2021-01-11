using EvidentInstruction.Web.Models.Factory.Browser;
using EvidentInstruction.Web.Models.Factory.Browser.Interfaces;
using EvidentInstruction.Web.Models.Settings;
using EvidentInstruction.Web.Models.Settings.Interfaces;
using System;

namespace EvidentInstruction.Web.Controllers
{
    public class BrowserController
    {
        [ThreadStatic]
        private static IBrowser _browser;

        private BrowserController() { }

        public static IBrowser GetBrowser()
        {
            if (_browser == null)
            {
                _browser = new Chrome();
                return _browser;
            }
            return _browser;
        }

        public IBrowser Create(ISetting setting)
        {
            if (_browser == null)
            {
                switch (((BrowserSetting)setting).BrowserType)
                {
                    case Infrastructures.BrowserType.CHROME:
                        {
                            _browser = new Chrome(setting);
                            return _browser;
                        }
                }
            }
            return _browser;
        }
    }
}
