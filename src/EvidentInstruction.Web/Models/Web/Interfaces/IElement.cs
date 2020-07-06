namespace AlfaBank.AFT.Core.Models.Web.Interfaces
{
    public interface IElement
    {
        string Name { get; }
        void SetDriver(Driver webDriver);

        void MoveTo();
        string GetText();
        string GetValue();
        string GetAttribute(string name);

        void PressKey(string key);

        bool IsTextContains(string text);
        bool IsTextEquals(string text);

        bool IsTextChange(string text);
        bool IsValueChange(string text);

        bool IsLoad();
        bool IsDisabled();
        bool IsEnabled();
        bool IsVisible();
        bool IsInvisible();
        bool IsSelected();
        bool IsNotSelected();
        bool IsEditable();
    }
}
