using System;
using OpenQA.Selenium;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Molder.Web.Exceptions;

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
            try
            {
                Alert.Accept();
            }
            catch (Exception ex)
            {
                throw new AlertException($"Accept in alert is return error with message {ex.Message}");
            }
        }

        public void SendDismiss()
        {
            try
            {
                Alert.Dismiss();
            }
            catch (Exception ex)
            {
                throw new AlertException($"Dismiss in alert is return error with message {ex.Message}");
            }
        }

        public void SendKeys(string keys)
        {
            try
            {
                Alert.SendKeys(keys);
            }
            catch (Exception ex)
            {
                throw new AlertException($"Send keys \"{keys}\" in alert is return error with message {ex.Message}");
            }
        }

        public void SetAuth(string login, string password)
        {
            try
            {
                Alert.SetAuthenticationCredentials(login, password);
            }
            catch (Exception ex)
            {
                throw new AlertException($"Authentication credentials ({login},{password}) in alert is return error with message {ex.Message}");
            }
        }
    }
}