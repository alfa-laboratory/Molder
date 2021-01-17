namespace EvidentInstruction.Web.Models.PageObject.Models.Alert.Interfaces
{
    public interface IAlert
    {
        string Text { get; }
        void Accept();
        void Dismiss();
        void SendKeys(string keys);
        void SetAuth(string login, string password);
    }
}