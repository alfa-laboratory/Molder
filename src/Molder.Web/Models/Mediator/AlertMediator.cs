using Molder.Web.Infrastructures;
using OpenQA.Selenium;
using Polly;
using System;

namespace Molder.Web.Models.Mediator
{
    public class AlertMediator : Mediator
    {
        public AlertMediator(int? timeout)
        {
            retryPolicy = Policy
                .Handle<NoAlertPresentException>()
                .Retry(CommandSetting.RETRY);

            waitAndRetryPolicy = Policy
                .Handle<NoAlertPresentException>()
                .WaitAndRetry(CommandSetting.RETRY,
                    retryAttempt => Math.Pow(2, retryAttempt) <= DefaultSetting.BROWSER_TIMEOUT
                    ? TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    : TimeSpan.FromSeconds(timeout ?? DefaultSetting.ELEMENT_TIMEOUT));
        }
    }
}
