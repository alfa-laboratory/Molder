using AlfaBank.AFT.Core.Models.Context;
using ReportPortal.Shared;
using System;
using AlfaBank.AFT.Core.Models.Web;
using TechTalk.SpecFlow;
using OpenQA.Selenium;

namespace AlfaBank.AFT.Core.Library.Web
{
    [Binding]
    public class WebHooks
    {
        private readonly ScenarioContext _scenario;
        private readonly WebContext _webContext;
        private readonly Driver _driver;

        public WebHooks(ScenarioContext scenario, WebContext context, Driver driver)
        {
            _scenario = scenario;
            _webContext = context;
            _driver = driver;
        }

        [AfterStep(new[] { "web", "Web" })]
        public void AfterStep()
        {
            if (_webContext.withReport)
            {
                LogMessage(_scenario.StepContext.StepInfo.Text);
            }
        }

        [AfterScenario(new[] {"web", "Web"})]
        public void AfterScenario()
        {
            if (_scenario.TestError != null)
            {
                LogMessage(_scenario.StepContext.StepInfo.Text);
            }
        }

        private void LogMessage(string text)
        {
            byte[] screenshot = null;
            string logMessage = string.Empty;
            bool IsError = false;
            try
            {
                if(_driver.WebDriver is null)
                {
                    return;
                }
                screenshot = ((ITakesScreenshot) (_driver.WebDriver)).GetScreenshot().AsByteArray;
                logMessage = text;
            }
            catch (UnhandledAlertException ex)
            {
                logMessage = $"Alert text is \"{ex.AlertText}\"";
                IsError = true;
            }
            catch(Exception ex)
            {
                logMessage = $"\"Проблемы c {ex.Message}\"";
            }

            if (IsError)
            {
                Log.Message(new ReportPortal.Client.Requests.AddLogItemRequest
                {
                    Level = ReportPortal.Client.Models.LogLevel.Error,
                    Time = DateTime.UtcNow,
                    Text = $"{logMessage}"
                });
            }
            else
            {
                Log.Message(new ReportPortal.Client.Requests.AddLogItemRequest
                {
                    Level = ReportPortal.Client.Models.LogLevel.Info,
                    Time = DateTime.UtcNow,
                    Text = $"{logMessage}",
                    Attach = new ReportPortal.Client.Models.Attach
                    {
                        Name = "Screenshot",
                        MimeType = "image/png",
                        Data = screenshot
                    }
                });
            }
        }
    }
}