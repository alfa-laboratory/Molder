using EvidentInstruction.Web.Models.PageObject.Attributes;
using EvidentInstruction.Web.Models.PageObject.Models.Page;
using PageObject.Elements.Blocks;
using PageObject.Elements.Elements;

namespace PageObject.Pages
{
    [Page(Name = "InternetHerokuapp", Url = "http://the-internet.herokuapp.com/")]
    public class InternetHerokuapp : Page
    {
        [Block(Name = "Блок А", Locator = "xpath blockA")]
        BlockA blockA;

        [Block(Name = "Блок Б", Locator = "xpath blockB", Optional = true)]
        BlockB blockB;

        [Block(Name = "Блок С", Locator = "xpath blockC")]
        BlockC blockC;

        [Element(Name = "tablePage", Locator = "tableP xpath")]
        Table pTable;

        [Element(Name = "inputPage", Locator = "inputP xpath", Optional = true)]
        Input pInput;
    }
}
