using Molder.Web.Models.Mediator;
using Molder.Web.Models.Providers;
using Molder.Web.Models.Settings;
using System;
using System.Threading;

namespace Molder.Web.Models.PageObjects.Alerts
{
    public class Alert : IAlert
    {
        private AsyncLocal<IMediator> _mediator = new AsyncLocal<IMediator> { Value = null };
        private IAlertProvider _alertProvider = null;

        public string Text => _alertProvider.Text;

        public Alert(IDriverProvider driverProvider)
        {
            _mediator.Value = new AlertMediator((driverProvider.Settings as BrowserSetting).ElementTimeout);
            _alertProvider = (IAlertProvider)_mediator.Value.Wait(() => driverProvider.GetAlert());
        }

        public void Accept()
        {
            _alertProvider.SendAccept();
        }

        public void Dismiss()
        {
            _alertProvider.SendDismiss();
        }

        public void SendKeys(string keys)
        {
            _alertProvider.SendKeys(keys);
        }

        public void SetAuth(string login, string password)
        {
            _alertProvider.SetAuth(login, password);
        }
    }
}
