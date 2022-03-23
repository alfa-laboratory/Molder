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
    public class ScenarioHooks
    {
        [AfterScenario(Order = int.MinValue)]
        public void AfterScenario(FeatureContext featureContext, ScenarioContext scenarioContext, Report report)
        {
            var feature = (report.Current.ReportTemplates() as List<Feature>)?.Find(f => f.Name.Equals(featureContext.FeatureInfo.Title));

            var status = scenarioContext.ScenarioExecutionStatus switch
            {
                ScenarioExecutionStatus.TestError or ScenarioExecutionStatus.BindingError or ScenarioExecutionStatus
                    .UndefinedStep => Status.Fail,
                ScenarioExecutionStatus.Skipped => Status.Ignore,
                ScenarioExecutionStatus.OK => Status.Pass,
                _ => Status.Pass
            };
            
            var match = scenarioContext.Tags()
                .FirstOrDefault(t => Regex.IsMatch(t, TaskPattern.Get.Order(), RegexOptions.IgnoreCase));
            
            string orderId = null;
            if (match is not null)
            {
                orderId = Regex.Match(match, TaskPattern.Get.Order(), RegexOptions.IgnoreCase).Groups[3].Value;
            }
            
            var scenario = new Scenario()
            {
                Name = scenarioContext.ScenarioInfo.Title,
                OrderId = orderId is null ? null : int.Parse(orderId),
                Status = status,
                Error = scenarioContext.TestError?.Message
            };

            if (feature is null) return;
            
            feature.Scenarios ??= new List<Scenario>();
            (feature.Scenarios as List<Scenario>)?.Add(scenario);
        }
    }
}