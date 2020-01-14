namespace AlfaBank.AFT.Core.Models.Web.Elements
{
    public class AElement : ClickElement
    {
        public AElement(string name, string xpath) : base(name, xpath) { }

        public string GetHref()
        {
            if (IsEnabled() && IsVisible())
            {
                var element = GetWebElement();
                return element.GetAttribute("href");
            }
            return null;
        }
    }
}
