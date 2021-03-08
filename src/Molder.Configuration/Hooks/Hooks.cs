using Molder.Configuration.Helpers;
using Molder.Configuration.Models;
using Molder.Configuration.Extension;
using Molder.Controllers;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TechTalk.SpecFlow;
using Molder.Models.Directory;
using System.Threading;

namespace Molder.Configuration.Hooks
{
    [ExcludeFromCodeCoverage]
    [Binding]
    internal class Hooks : Steps
    {
        private VariableController variableController;
        private readonly ScenarioContext scenarioContext;

        private static IDirectory BinDirectory = new BinDirectory();
        private static AsyncLocal<IOptions<IEnumerable<ConfigFile>>> config = new AsyncLocal<IOptions<IEnumerable<ConfigFile>>>();

        public Hooks(VariableController variableController, ScenarioContext scenarioContext)
        {
            this.variableController = variableController;
            this.scenarioContext = scenarioContext;
        }

        [BeforeFeature(Order = -100000)]
        public static void BeforeFeature(FeatureContext featureContext, VariableController variableController)
        {
            BinDirectory.Create();
            var configuration = ConfigurationFactory.Create(BinDirectory);
            config.Value = ConfigOptionsFactory.Create(configuration);

            var tags = TagHelper.GetTagsBy(featureContext);
            variableController.AddConfig(config.Value, tags);
        }

        [BeforeScenario(Order = -100000)]
        public void BeforeScenario()
        {
            BinDirectory.Create();
            var configuration = ConfigurationFactory.Create(BinDirectory);
            config.Value = ConfigOptionsFactory.Create(configuration);

            var tags = TagHelper.GetTagsBy(scenarioContext);
            variableController.AddConfig(config.Value, tags);
        }
    }
}