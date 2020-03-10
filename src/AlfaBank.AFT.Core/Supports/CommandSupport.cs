using System;
using System.Diagnostics;
using AlfaBank.AFT.Core.Models.Web;
using OpenQA.Selenium;

namespace AlfaBank.AFT.Core.Supports
{
    public class CommandSupport
    {
        private const int retry = 3;
        private readonly TimeSpan _interval = TimeSpan.FromMilliseconds(25);

        protected Driver _driverSupport;

        public CommandSupport(Driver driverSupport)
        {
            this._driverSupport = driverSupport;
        }

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

        public object WaitCommand<TResult>(Func<TResult> command)
        {
            return waitCommand(command);
        }

        private object waitCommand<TResult>(Func<TResult> command)
        {
            var stopwatch = new Stopwatch();

            while (stopwatch.ElapsedMilliseconds <= _driverSupport.Timeout)
            {
                var objResult = command();
                if (objResult != null)
                    return objResult;
                System.Threading.Thread.Sleep(_interval);
            }
            return null;
        }
    }
}