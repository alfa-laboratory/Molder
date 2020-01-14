namespace AlfaBank.AFT.Core.Models.Web.Elements
{
    public class FrameElement : Element
    {
        public FrameElement(string name, string xpath) : base(name, xpath) { }

        public void Switch()
        {
            var element = GetWebElement();
            this._driverSupport.WebDriver.SwitchTo().Frame(element);
        }
    }
}
