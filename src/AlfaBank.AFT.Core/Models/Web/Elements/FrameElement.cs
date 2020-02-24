using OpenQA.Selenium;
using System;

namespace AlfaBank.AFT.Core.Models.Web.Elements
{
    public class FrameElement : Element
    {
        public FrameElement(string name, string xpath) : base(name, xpath) { }

        public virtual void Switch()
        {
            try
            {
                var element = GetWebElement();

                this._driverSupport.WebDriver.SwitchTo().Frame(element);
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException(ex.Message);
            }
            catch (NoSuchElementException ex)
            {
                throw new NoSuchElementException(ex.Message);
            }
        }
    }
}
