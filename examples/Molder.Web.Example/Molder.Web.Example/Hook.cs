using PageObject;
using TechTalk.SpecFlow;

namespace Molder.Web.Example
{
    [Binding]
    public static class Hook
    {
#if DEBUG
        [BeforeTestRun(Order = int.MinValue)]
        public static void PageObjectInit()
        {
            HealthCheck.IsHealthCheck();
        }
#endif 
    }
}