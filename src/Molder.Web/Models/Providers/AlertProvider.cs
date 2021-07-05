using OpenQA.Selenium;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Molder.Web.Models.Providers
{
    [ExcludeFromCodeCoverage]
    public class AlertProvider : IAlertProvider
    {
        #region Alert

        private AsyncLocal<IAlert> _alert = new AsyncLocal<IAlert>{ Value = null };
        public IAlert Alert
        {
            get => _alert.Value;
            set => _alert.Value = value;
        }

        #endregion
        
        public string Text => Alert.Text;

        public void SendAccept()
        {
            Alert.Accept();
        }

        public void SendDismiss()
        {
            Alert.Dismiss();
        }

        public void SendKeys(string keys)
        {
            Alert.SendKeys(keys);
        }

        public void SetAuth(string login, string password)
        {
            Alert.SetAuthenticationCredentials(login, password);
        }
    }
}