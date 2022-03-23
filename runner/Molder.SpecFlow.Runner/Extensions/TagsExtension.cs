using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Molder.SpecFlow.Runner.Extensions
{
    public static class TagsExtension
    {
        public static IEnumerable<string> Tags(this SpecFlowContext context)
        {
            if (context is FeatureContext featureContext)
                return Get(featureContext);
            return Get(context as ScenarioContext);
        }
        
        private static IEnumerable<string> Get(IFeatureContext context)
        {
            return context.FeatureInfo.Tags;
        }

        private static IEnumerable<string> Get(IScenarioContext context)
        {
            return context.ScenarioInfo.Tags;
        }
    }
}