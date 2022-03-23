using System.Collections.Generic;
using System.Linq;
using Molder.SpecFlow.Runner.Infrastructure;
using Molder.SpecFlow.Runner.Models.ReportTemplate;

namespace Molder.SpecFlow.Runner.Extensions
{
    public static class ScenarioExtension
    {
        public static Status GetStatus(this IEnumerable<Scenario> _scenarios)
        {
            if(_scenarios.All(s => s.Status is Status.Pass))
                return Status.Pass;
            if (_scenarios.All(s => s.Status == Status.Ignore)) 
                return Status.Ignore;
            return _scenarios.Any(s => s.Status == Status.Fail) ? Status.Fail : Status.Pass;
        }
    }
}