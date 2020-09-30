using EvidentInstruction.Web.Exceptions;
using EvidentInstruction.Web.Infrastructures;
using EvidentInstruction.Web.Models.PageObject.Models.Interfaces;
using OpenQA.Selenium;
using System;
using System.Diagnostics;

namespace EvidentInstruction.Web.Models.PageObject.Models
{
    public class ElementMediator : IMediator
    {
        IElement _element;

        public ElementMediator(IElement element)
        {
            this._element = element;
            this._element.SetMediator(this);
        }

        public void Execute(object sender, Action action)
        {
            var attempts = 0;
            var res = false;
            while (attempts < CommandSetting.RETRY && !res)
            {
                try
                {
                    action();
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
            throw new ElementExecuteCommandException(_element.Name, $"{action.Method} not available. Sorry");
        }

        public object Execute<TResult>(object sender, Func<TResult> action)
        {
            var attempts = 0;
            while (attempts < CommandSetting.RETRY)
            {
                try
                {
                    return action();
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
            throw new ElementExecuteCommandException(_element.Name, $"{action.Method} not available. Sorry");
        }

        public object Wait<TResult>(object sender, Func<TResult> action)
        {
            var stopwatch = new Stopwatch();

            while (stopwatch.ElapsedMilliseconds <= DefaultSetting.BROWSER_TIMEOUT)
            {
                var act = action();
                if (act != null) return act;
                System.Threading.Thread.Sleep(CommandSetting.INTERVAL);
            }
            throw new ElementExecuteCommandException(_element.Name, $"{action.Method} not available. Wait");
        }
    }
}
