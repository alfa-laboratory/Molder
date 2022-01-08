using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Molder.Web.Models.PageObjects.Elements
{
    public class File : Input
    {
        public File(string name, string locator, bool optional) : base(name, locator, optional) { }

        public override void SetText(string text)
        {
            if (Driver.GetDriver() is IAllowsFileDetection allowsDetection)
            {
                allowsDetection.FileDetector = new LocalFileDetector();
            }

            mediator.Execute(() => ElementProvider.SendKeys(text));
        }
    }
}