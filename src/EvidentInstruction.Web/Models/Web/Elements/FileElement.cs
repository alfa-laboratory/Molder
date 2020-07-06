using OpenQA.Selenium;
using System;

namespace AlfaBank.AFT.Core.Models.Web.Elements
{
    public class FileElement : Element
    {
        public FileElement(string name, string xpath) : base(name, xpath) { }

        public virtual void SetText(string text)
        {
            try
            {
                var element = GetWebElement();

                element.SendKeys(text);
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