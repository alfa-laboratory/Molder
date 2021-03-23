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

        public ScenarioHooks(VariableController variableController, 
            ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            this.controller = variableController;
        }

        [BeforeScenario(Order = -1000000)]
        public void BeforeScenario(FeatureContext feature, ScenarioContext scenario)
        {
            ConfigurationExtension.Instance.Configuration = ConfigurationExtension.Instance.Configuration ?? ConfigurationFactory.Create(BinDirectory);
            config.Value = ConfigOptionsFactory.Create(ConfigurationExtension.Instance.Configuration);
        
            var tags = TagHelper.GetTagsBy(feature);
            tags.Concat(TagHelper.GetTagsBy(scenario));
            controller.AddConfig(config.Value, tags);
        }
    }
}