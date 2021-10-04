using Molder.Models.Configuration;
using Molder.ReportPortal.Helpers;
using Molder.ReportPortal.Models;
using Molder.ReportPortal.Extensions;
using TechTalk.SpecFlow;

namespace Molder.ReportPortal.Hooks
{
    [Binding]
    public class Hooks : Steps
    {
        [BeforeTestRun(Order = -9999999)]
        public static void InitializeConfiguration()
        {
            var settings = ConfigOptionsFactory.Create(ConfigurationExtension.Instance.Configuration);
            LoggerSettings.Settings = settings.Value;
            
            Molder.Helpers.Log.LoggerFactory.ConfigureLogger();
        }
    }
}