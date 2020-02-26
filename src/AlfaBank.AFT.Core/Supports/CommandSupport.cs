using System;
using OpenQA.Selenium;

namespace AlfaBank.AFT.Core.Supports
{
    public class CommandSupport
    {
        private const int retry = 3;

        public void SendCommand(Action webSupport)
        {
            var attempts = 0;
            var res = false;
            while (attempts < retry && !res)
            {
                try
                {
                    webSupport();
                    res = true;
                }
                catch (StaleElementReferenceException)
                {
                    attempts++;
                }
                catch (ElementClickInterceptedException)
                {
                    attempts++;
                }
                catch (ElementNotInteractableException)
                {
                    attempts++;
                }
                catch (InvalidElementStateException)
                {
                    attempts++;
                }
            }
        }

        public object SendCommand<TResult>(Func<TResult> webSupport)
        {
            var attempts = 0;
            while (attempts < retry)
            {
                try
                {
                    return webSupport();
                }
                catch (StaleElementReferenceException)
                {
                    attempts++;
                }
                catch (ElementClickInterceptedException)
                {
                    attempts++;
                }
                catch (ElementNotInteractableException)
                {
                    attempts++;
                }
                catch (InvalidElementStateException)
                {
                    attempts++;
                }
            }
            return null;
        }
    }
}