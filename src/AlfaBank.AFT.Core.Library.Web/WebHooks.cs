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
                LogScreenshot(_scenario.StepContext.StepInfo.Text);
            }
        }

        private void LogScreenshot(string text)
        {
            var screenshot = ((ITakesScreenshot)(_driver.WebDriver)).GetScreenshot().AsByteArray;
            Log.Message(new ReportPortal.Client.Requests.AddLogItemRequest
            {
                Level = ReportPortal.Client.Models.LogLevel.Debug,
                Time = DateTime.UtcNow,
                Text = $"{text}",
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