using Molder.Web.Infrastructures;
using OpenQA.Selenium;
using Polly;
using System;

namespace Molder.Web.Models.PageObject.Models.Mediator
{
    public class FrameMediator : Mediator
    {
        public FrameMediator(int? timeout)
        {
            retryPolicy = Policy
                .Handle<StaleElementReferenceException>()
                .Or<ElementClickInterceptedException>()
                .Or<ElementNotInteractableException>()
                .Or<InvalidElementStateException>()
                .Or<NoSuchFrameException>()
                .Retry(CommandSetting.RETRY);

            waitAndRetryPolicy = Policy
                .Handle<StaleElementReferenceException>()
                .Or<ElementClickInterceptedException>()
                .Or<ElementNotInteractableException>()
                .Or<InvalidElementStateException>()
                .Or<NoSuchFrameException>()
                .WaitAndRetry(CommandSetting.RETRY,
                    retryAttempt => Math.Pow(2, retryAttempt) <= DefaultSetting.BROWSER_TIMEOUT
                    ? TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    : TimeSpan.FromSeconds(timeout ?? DefaultSetting.ELEMENT_TIMEOUT));
        }
    }
}