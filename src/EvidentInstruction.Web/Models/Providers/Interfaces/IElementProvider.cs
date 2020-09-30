using OpenQA.Selenium;
using System.Collections.ObjectModel;
using System.Drawing;

namespace EvidentInstruction.Web.Models.Providers.Interfaces
{
    public interface IElementProvider
    {
        bool Displayed { get; }
        bool Selected { get; }
        bool Enabled { get; }
        Point Location { get; }
        string Text { get; }
        string Tag { get; }

        void Clear();
        void Click();
        string GetAttribute(string name);
        string GetCss(string name);
        void SendKeys(string keys);

        IElementProvider FindElement(By by);
        ReadOnlyCollection<IElementProvider> FindElements(By by);
    }
}
