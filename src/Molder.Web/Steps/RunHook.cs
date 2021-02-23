using Molder.Web.Models;
using TechTalk.SpecFlow;

namespace Molder.Web.Steps
{
    [Binding]
    internal class RunHook : TechTalk.SpecFlow.Steps
    {
        [BeforeFeature(Order = -25000)]
        public static void BeforeFeature()
        {
            TreePages.Get();
        }
    }
}
