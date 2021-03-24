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
using Molder.Models.Configuration;
using System;
using System.Linq;
using Molder.Helpers;
using Microsoft.Extensions.Logging;

namespace Molder.Configuration.Hooks
{
    [ExcludeFromCodeCoverage]
    [Binding]
    public class ScenarioHooks : Steps
    {
        private VariableController controller;
        private readonly ScenarioContext scenarioContext;
        private readonly FeatureContext featureContext;

        private static IDirectory BinDirectory = new BinDirectory();
        private static AsyncLocal<IOptions<IEnumerable<ConfigFile>>> config = new AsyncLocal<IOptions<IEnumerable<ConfigFile>>>();

        public ScenarioHooks(VariableController variableController)
        {
            this.controller = variableController;
        }

        [BeforeFeature(Order = -1000000)]
        public static void BeforeFeature(FeatureContext featureContext, VariableController variableController)
        {
            BinDirectory.Create();
            ConfigurationExtension.Instance.Configuration = ConfigurationFactory.Create(BinDirectory);
            config.Value = ConfigOptionsFactory.Create(ConfigurationExtension.Instance.Configuration);

            var tags = TagHelper.GetTagsBy(featureContext);
            variableController.AddConfig(config.Value, tags);
        }

        [BeforeScenario(Order = -1000000)]
        public void BeforeScenario(FeatureContext feature, ScenarioContext scenario)
        {
            ConfigurationExtension.Instance.Configuration = ConfigurationExtension.Instance.Configuration ?? ConfigurationFactory.Create(BinDirectory);
            config.Value = ConfigOptionsFactory.Create(ConfigurationExtension.Instance.Configuration);

            var tags = TagHelper.GetTagsBy(scenario);
            controller.AddConfig(config.Value, tags);
        }

        [AfterScenario(Order = -1000000)]
        public void AfterScenario()
        {
            Log.Logger().LogInformation("Dictionary with variables is " + (controller.Variables.Any() ? "not empty" : "empty"));
            if (!controller.Variables.Any()) return;

            foreach (var variable in controller.Variables)
            {
                if(variable.Value.TypeOfAccess == Molder.Infrastructures.TypeOfAccess.Local)
                {
                    controller.Variables.TryRemove(variable.Key, out var value);
                }
            }
        }
    }
}