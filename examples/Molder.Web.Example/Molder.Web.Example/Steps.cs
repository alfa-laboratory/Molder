using System;
using System.Linq;
using Molder.Web.Controllers;
using TechTalk.SpecFlow;

namespace Molder.Web.Example
{
    [Binding]
    public sealed class Steps
    {
        [When(@"GetCollection")]
        public void GetCollection()
        {
            var tst = BrowserController.GetBrowser().GetCurrentPage().GetCollection("subContent");
            foreach (var el in tst)
            {
                Console.WriteLine(el.Text);
            }
        }
    }
}