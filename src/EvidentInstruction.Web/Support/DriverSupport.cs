using AlfaBank.AFT.Core.Models.Web;
using OpenQA.Selenium;

namespace AlfaBank.AFT.Core.Supports
{
    public class DriverSupport
    {
        private readonly Driver _driverSupport;

        public DriverSupport(Driver driverSupport)
        {
            this._driverSupport = driverSupport;
        }

        public IAlert GetAlert()
        {
            try
            {
                return _driverSupport.WebDriver.SwitchTo().Alert();
            }
            catch(NoAlertPresentException)
            {
                return null;
            }
        }

        public void SwitchFrameBy(int number)
        {
            this._driverSupport.WebDriver.SwitchTo().Frame(number);
        }

        public void SwitchToDefaultContent()
        {
            this._driverSupport.WebDriver.SwitchTo().DefaultContent();
        }

        public void SwitchToParentFrame()
        {
            this._driverSupport.WebDriver.SwitchTo().ParentFrame();
        }
    }
}
