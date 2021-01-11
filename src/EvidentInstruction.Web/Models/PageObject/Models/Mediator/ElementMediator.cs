using EvidentInstruction.Web.Infrastructures;
using EvidentInstruction.Web.Models.PageObject.Models.Mediator.Interfaces;
using OpenQA.Selenium;
using Polly;
using Polly.Retry;
using System;

namespace EvidentInstruction.Web.Models.PageObject.Models.Mediator
{
    public class ElementMediator : IMediator
    {
        private RetryPolicy retryPolicy = null;

        private RetryPolicy waitAndRetryPolicy = null;

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

        public void Execute(Action action)
        {
            retryPolicy.Execute(action);
        }

        public object Execute<TResult>(Func<TResult> action)
        {
            return retryPolicy.Execute(action);
        }

        public object Wait<TResult>(Func<TResult> action)
        {
            return waitAndRetryPolicy.Execute(action);
        }
    }
}
