using Molder.Web.Infrastructures;
using OpenQA.Selenium;
using Polly;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Molder.Web.Models.Mediator
{
    [ExcludeFromCodeCoverage]
    public class FrameMediator : Mediator
    {
        public FrameMediator(long? timeout)
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
                    retryAttempt => Math.Pow(2, retryAttempt) <= Constants.TIMEOUT
                    ? TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    : TimeSpan.FromSeconds(timeout ?? Constants.TIMEOUT));
        }
    }
}