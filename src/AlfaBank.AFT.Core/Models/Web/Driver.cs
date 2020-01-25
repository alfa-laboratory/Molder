using OpenQA.Selenium;
using System;
using System.Threading;

namespace AlfaBank.AFT.Core.Models.Web
{
    public class Driver
    {
        private ThreadLocal<IWebDriver> _driver { get; set; } = new ThreadLocal<IWebDriver>();

        public IWebDriver WebDriver
        {
            get
            {
                if (!_driver.IsValueCreated)
                {
                    throw new NullReferenceException(
                        "Драйвер не был инициализирован. Проинициализируйте драйвер перед его использованием.");
                }

                return _driver.Value;
            }
            set { _driver.Value = value; }
        }

        public int Timeout { get; set; } = 500;
    }
}
