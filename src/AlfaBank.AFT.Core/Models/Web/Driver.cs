using OpenQA.Selenium;

namespace AlfaBank.AFT.Core.Models.Web
{
    public class Driver
    {
        public IWebDriver WebDriver { get; set; } = null;
        public int Timeout { get; set; } = 500;
    }
}
