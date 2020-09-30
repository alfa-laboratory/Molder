using EvidentInstruction.Web.Models.Providers.Interfaces;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EvidentInstruction.Web.Models.Providers
{
    public class DriverProvider : IDriverProvider
    {
        [ThreadStatic]
        private IWebDriver _driver = null;

        public string PageSource => _driver.PageSource;

        public string Title => _driver.Title;

        public string Url => _driver.Url;

        public string CurrentWindowHandle => _driver.CurrentWindowHandle;

        public ReadOnlyCollection<string> WindowHandles => _driver.WindowHandles;

        public void SetDriver(IWebDriver driver)
        {
            _driver = driver;
        }

        public void Back()
        {
            _driver.Navigate().Back();
        }

        public bool Close()
        {
            try
            {
                _driver.Close();
                return true;
            }catch(Exception)
            {
                return false;
            }
        }

        public void Forward()
        {
            _driver.Navigate().Forward();
        }

        public IElementProvider GetActiveElement()
        {
            var element = _driver.SwitchTo().ActiveElement();
            return new ElementProvider()
            {
                Element = element
            };
        }

        public IAlertProvider GetAlert()
        {
            var alert = _driver.SwitchTo().Alert();
            return new AlertProvider()
            {
                Alert = alert
            };
        }

        public IDriverProvider GetDefaultFrame()
        {
            var driver = _driver.SwitchTo().DefaultContent();
            return new DriverProvider()
            {
                _driver = driver
            };
        }

        public IElementProvider GetElement(By by)
        {
            var element = _driver.FindElement(by);
            return new ElementProvider()
            {
                Element = element
            };
        }

        public ReadOnlyCollection<IElementProvider> GetElements(By by)
        {
            var elements = _driver.FindElements(by);
            var listElement = new List<IElementProvider>();
            foreach (var element in elements)
            {
                listElement.Add(new ElementProvider()
                {
                    Element = element
                });
            }
            return listElement.AsReadOnly();
        }

        public IDriverProvider GetFrame(int id)
        {
            var driver = _driver.SwitchTo().Frame(id);
            return new DriverProvider()
            {
                _driver = driver
            };
        }

        public IDriverProvider GetFrame(string name)
        {
            var driver = _driver.SwitchTo().Frame(name);
            return new DriverProvider()
            {
                _driver = driver
            };
        }

        public IDriverProvider GetFrame(By by)
        {
            var element = _driver.FindElement(by);
            var driver = _driver.SwitchTo().Frame(element);
            return new DriverProvider()
            {
                _driver = driver
            };
        }

        public bool GoToUrl(string url)
        {
            throw new NotImplementedException();
        }

        public bool GoToUrl(Uri url)
        {
            throw new NotImplementedException();
        }

        public void Maximize()
        {
            throw new NotImplementedException();
        }

        public bool Quit()
        {
            try
            {
                _driver.Quit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Refresh()
        {
            try
            {
                _driver.Navigate().Refresh();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Screenshot Screenshot()
        {
            throw new NotImplementedException();
        }

        public bool WindowSize(int width, int height)
        {
            throw new NotImplementedException();
        }
    }
}
