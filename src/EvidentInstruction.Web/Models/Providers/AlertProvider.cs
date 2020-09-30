using EvidentInstruction.Web.Models.Providers.Interfaces;
using OpenQA.Selenium;
using System;

namespace EvidentInstruction.Web.Models.Providers
{
    public class AlertProvider : IAlertProvider
    {
        [ThreadStatic]
        public IAlert Alert = null;

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
