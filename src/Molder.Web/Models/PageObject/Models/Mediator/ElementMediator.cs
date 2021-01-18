using Molder.Web.Infrastructures;
using OpenQA.Selenium;
using Polly;
using System;

namespace Molder.Web.Models.PageObject.Models.Mediator
{
    public class ElementMediator : Mediator
    {
        public ElementMediator(int? timeout)
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
                    retryAttempt => Math.Pow(2, retryAttempt) <= DefaultSetting.BROWSER_TIMEOUT
                    ? TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    : TimeSpan.FromSeconds(timeout ?? DefaultSetting.ELEMENT_TIMEOUT));
        }
    }
}
