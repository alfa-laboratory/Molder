namespace AlfaBank.AFT.Core.Models.Web.Interfaces
{
    public interface IElement
    {
        void SetDriver(Driver webDriver);

        void MoveTo();
        string GetText();
        string GetValue();
        string GetAttribute(string name);

        void PressKey(string key);

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
