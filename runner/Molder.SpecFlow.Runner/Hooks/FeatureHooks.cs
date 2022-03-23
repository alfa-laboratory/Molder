using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Molder.SpecFlow.Runner.Extensions;
using Molder.SpecFlow.Runner.Infrastructure;
using Molder.SpecFlow.Runner.Models.ReportTemplate;
using TechTalk.SpecFlow;

namespace Molder.SpecFlow.Runner.Hooks
{
    [Binding]
    public class FeatureHooks
    {
        [BeforeFeature(Order = int.MinValue)]
        public static void BeforeFeature(FeatureContext context, Report report)
        {
            var match = context.Tags()
                .FirstOrDefault(t => Regex.IsMatch(t, TaskPattern.Get.Pattern(), RegexOptions.IgnoreCase));
            
            string task = null;
            if (match is not null)
            {
                task = Regex.Match(match, TaskPattern.Get.Pattern(), RegexOptions.IgnoreCase).Groups[2].Value;
            }

            var feature = new Feature
            {
                Name = context.FeatureInfo.Title,
                Task = task
            };
            
            (report.Current.ReportTemplates() as List<Feature>)?.Add(feature);
        }

        [AfterFeature(Order = int.MinValue)]
        public static void AfterFeature(FeatureContext context, Report report)
        {
            var feature = (report.Current.ReportTemplates() as List<Feature>)?.Find(f => f.Name.Equals(context.FeatureInfo.Title));
            feature.Status = feature.Scenarios.GetStatus();
            feature.Scenarios = feature.Scenarios.OrderByDescending(s => s.OrderId.HasValue).ThenBy(s => s.OrderId);
        }
    }
}