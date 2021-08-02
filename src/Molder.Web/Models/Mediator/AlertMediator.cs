using Molder.Web.Infrastructures;
using OpenQA.Selenium;
using Polly;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Molder.Web.Models.Mediator
{
    [ExcludeFromCodeCoverage]
    public class AlertMediator : Mediator
    {
        public AlertMediator(long? timeout)
        {
            retryPolicy = Policy
                .Handle<NoAlertPresentException>()
                .Retry(CommandSetting.RETRY);

            waitAndRetryPolicy = Policy
                .Handle<NoAlertPresentException>()
                .WaitAndRetry(CommandSetting.RETRY,
                    retryAttempt => Math.Pow(2, retryAttempt) <= Constants.TIMEOUT
                    ? TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    : TimeSpan.FromSeconds(timeout ?? Constants.TIMEOUT));
        }
    }
}