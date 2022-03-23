using System.IO;
using System.Reflection;
using BoDi;
using Molder.SpecFlow.Runner.Models.ReportTemplate;
using Newtonsoft.Json;
using TechTalk.SpecFlow;

namespace Molder.SpecFlow.Runner.Hooks
{
    [Binding]
    public class TestRunHooks
    {
        [BeforeTestRun(Order = int.MinValue)]
        public static void BeforeTestRun(ObjectContainer testThreadContainer)
        {
            testThreadContainer.BaseContainer.Resolve<Report>();
        }
        
        [AfterTestRun(Order = int.MinValue)]
        public static void AfterTestRun(Report report)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var json = JsonConvert.SerializeObject(report.Current.ReportTemplates());
            File.WriteAllText($"{path}\\report.json", json);
        }
    }
}