using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using TechTalk.SpecFlow;

namespace Molder.Configuration.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class TagHelper
    {
        public static IEnumerable<string> GetTagsBy(FeatureContext feature)
        {
            var featureTags = feature.FeatureInfo.Tags;
            return featureTags.ToList();
        }

        public static IEnumerable<string> GetTagsBy(ScenarioContext scenario)
        {
            var scenarioTags = scenario.ScenarioInfo.Tags;
            return scenarioTags.ToList();
        }
    }
}
