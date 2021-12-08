using Microsoft.Extensions.Logging;
using Molder.Helpers;
using Molder.Kafka.Helpers;
using Molder.Kafka.Infrastructures;
using Molder.Kafka.Models;
using Molder.Models.Configuration;
using TechTalk.SpecFlow;

namespace Molder.Kafka.Hooks
{
    [Binding]
    public class Hooks : TechTalk.SpecFlow.Steps
    {
        [BeforeTestRun(Order = -8000000)]
        public static void InitializeConfiguration()
        {
            var settings = ConfigOptionsFactory.Create(ConfigurationExtension.Instance.Configuration);
            if (settings.Value is null)
            {
                Log.Logger().LogInformation($@"appsettings is not contains {Constants.CONFIG_BLOCK} block. Standard settings selected.");
            }
            else
            {
                foreach (var setting in settings.Value)
                {
                    KafkaSettings.Settings.Add(setting.Name, setting);
                }
                
                Log.Logger().LogInformation($@"appsettings contains {Constants.CONFIG_BLOCK} block. Settings selected.");
            }
        }
    }
}