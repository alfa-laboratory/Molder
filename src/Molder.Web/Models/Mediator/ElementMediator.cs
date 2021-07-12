using Molder.Web.Infrastructures;
using OpenQA.Selenium;
using Polly;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Molder.Web.Models.Mediator
{
    [ExcludeFromCodeCoverage]
    public class ElementMediator : Mediator
    {
        public ElementMediator(long? timeout)
        {
            retryPolicy = Policy
                .Handle<StaleElementReferenceException>()
                .Or<ElementClickInterceptedException>()
                .Or<ElementNotInteractableException>()
                .Or<InvalidElementStateException>()
                .Retry(CommandSetting.RETRY);

            waitAndRetryPolicy = Policy
                .Handle<StaleElementReferenceException>()
                .Or<ElementClickInterceptedException>()
                .Or<ElementNotInteractableException>()
                .Or<InvalidElementStateException>()
                .WaitAndRetry(CommandSetting.RETRY,
                    retryAttempt => Math.Pow(2, retryAttempt) <= Constants.TIMEOUT
                    ? TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    : TimeSpan.FromSeconds(timeout ?? Constants.TIMEOUT));
        }
    }
}
