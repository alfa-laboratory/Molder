using EvidentInstruction.Configuration.Helpers;
using EvidentInstruction.Configuration.Models;
using EvidentInstruction.Configuration.Extension;
using EvidentInstruction.Controllers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TechTalk.SpecFlow;

namespace EvidentInstruction.Configuration.Hooks
{
    [ExcludeFromCodeCoverage]
    [Binding]
    internal class Hooks : Steps
    {
        private VariableController variableController;
        private readonly FeatureContext featureContext;
        private readonly ScenarioContext scenarioContext;

        [ThreadStatic]
        IOptions<IEnumerable<ConfigFile>> config;

        public Hooks(VariableController variableController,
            FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            this.variableController = variableController;
            this.featureContext = featureContext;
            this.scenarioContext = scenarioContext;

            var configuration = ConfigurationFactory.Create();
            config = ConfigOptionsFactory.Create(configuration);
        }

        [BeforeScenario(Order = -30000)]
        public void BeforeScenario()
        {
            var tags = TagHelper.GetAllTags(featureContext, scenarioContext);

            variableController.AddConfig(config, tags);
        }
    }
}
