using EvidentInstruction.Web.Controllers;
using EvidentInstruction.Web.Models.Settings;

namespace TestWeb
{
    class Program
    {
        static void Main(string[] args)
        {
            var pages = PageCollection.GetPages();

            BrowserController.GetBrowser();

            BrowserController.GetBrowser().SetCurrentPage("InternetHerokuapp");

            BrowserController.GetBrowser().Close();
        }
    }
}
