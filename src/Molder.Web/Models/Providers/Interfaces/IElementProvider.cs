using OpenQA.Selenium;
using System.Collections.ObjectModel;
using System.Drawing;

namespace Molder.Web.Models.Providers
{
    public interface IElementProvider
    {
        bool Displayed { get; }
        bool NotDisplayed { get; }
        bool Selected { get; }
        bool NotSelected { get; }
        bool Enabled { get; }
        bool Disabled { get; }
        bool Editabled { get; }
        bool NotEditabled { get; }
        bool Loaded { get; }
        bool NotLoaded { get; }
        
        Point Location { get; }
        string Text { get; }
        string Tag { get; }

        void Clear();
        void Click();
        string GetAttribute(string name);
        string GetCss(string name);
        void SendKeys(string keys);

        bool TextEqual(string text);
        bool TextContain(string text);
        bool TextMatch(string text);

        IElementProvider FindElement(By by);
        ReadOnlyCollection<IElementProvider> FindElements(By by);
    }
}
