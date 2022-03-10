using System;
using System.Linq;
using Molder.Web.Controllers;
using Molder.Web.Models.PageObjects.Blocks;
using TechTalk.SpecFlow;

namespace Molder.Web.Example
{
    [Binding]
    public sealed class Steps
    {

        [When(@"GetCollection")]
        public void GetCollection()
        {
            var tst = BrowserController.GetBrowser().GetCurrentPage().GetCollection("subContent").Cast<Block>();
            foreach (var el in tst)
            {
                var text = el.GetElement("Text").Text;
                Console.WriteLine(el.Text);
            }
        }
    }
}