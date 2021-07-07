using System;
using PageObject;
using TechTalk.SpecFlow;

namespace Molder.Web.Example
{
    [Binding]
    public static class Hook
    {
        [BeforeTestRun(Order = Int32.MinValue)]
        public static void PageObjectInit()
        {
            HealthCheck.IsHealthCheck();
        }
    }
}