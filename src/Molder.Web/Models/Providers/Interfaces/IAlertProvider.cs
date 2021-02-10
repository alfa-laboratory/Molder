namespace Molder.Web.Models.Providers
{
    public interface IAlertProvider
    {
        string Text { get; }

        void SendAccept();
        void SendDismiss();
        void SendKeys(string keys);
        void SetAuth(string login, string password);
    }
}
