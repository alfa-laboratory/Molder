namespace AlfaBank.AFT.Core.Models.Web.Elements
{
    public class FileElement : Element
    {
        public FileElement(string name, string xpath) : base(name, xpath) { }

        public virtual void SetText(string text)
        {
            if (IsEnabled() && IsVisible())
            {
                var element = this.GetWebElement();
                element.SendKeys(text);
            }
        }
    }
}