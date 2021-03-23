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

namespace Molder.Configuration.Hooks
{
    [ExcludeFromCodeCoverage]
    [Binding]
    internal class FeatureHooks : Steps
    {
        #region Придумать способ разделение Feature от Scenario. При статике сохраняется между сценариями содержимое VariableController (с содержимым первого пройденного сценария)
#if Feature
        private static AsyncLocal<VariableController> controller = new AsyncLocal<VariableController> { Value = null };

        private static IDirectory BinDirectory = new BinDirectory();
        private static AsyncLocal<IOptions<IEnumerable<ConfigFile>>> config = new AsyncLocal<IOptions<IEnumerable<ConfigFile>>>();

        [BeforeFeature(Order = -100000)]
        public static void BeforeFeature(FeatureContext featureContext, VariableController variableController)
        {
            controller.Value = variableController;
            BinDirectory.Create();
            ConfigurationExtension.Instance.Configuration = ConfigurationFactory.Create(BinDirectory);
            config.Value = ConfigOptionsFactory.Create(ConfigurationExtension.Instance.Configuration);
        
            var tags = TagHelper.GetTagsBy(featureContext);
            controller.Value.AddConfig(config.Value, tags);
        }
#endif
        #endregion
    }
}