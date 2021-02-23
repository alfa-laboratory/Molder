using OpenQA.Selenium;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Molder.Web.Models.Providers
{
    [ExcludeFromCodeCoverage]
    public class AlertProvider : IAlertProvider
    {
        private AsyncLocal<IAlert> _alert = new AsyncLocal<IAlert> { Value = null };

        public IAlert Alert
        {
            get => _alert.Value;
            set
            {
                _alert.Value = value;
            }
        }

        public string Text => _alert.Value.Text;

        public void SendAccept()
        {
            _alert.Value.Accept();
        }

        public void SendDismiss()
        {
            _alert.Value.Dismiss();
        }

        public void SendKeys(string keys)
        {
            _alert.Value.SendKeys(keys);
        }

        public void SetAuth(string login, string password)
        {
            _alert.Value.SetAuthenticationCredentials(login, password);
        }
    }
}
