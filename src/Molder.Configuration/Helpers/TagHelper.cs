using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using TechTalk.SpecFlow;

namespace Molder.Configuration.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class TagHelper
    {
        public static IEnumerable<string> GetAllTags(FeatureContext feature, ScenarioContext scenario)
        {
            var featureTags = feature.FeatureInfo.Tags;
            var scenarioTags = scenario.ScenarioInfo.Tags;

            return scenarioTags.Concat(featureTags).OrderBy(tag => tag).ToList();
        }
    }
}
