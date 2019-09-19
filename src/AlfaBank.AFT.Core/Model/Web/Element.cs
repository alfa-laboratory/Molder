using TechTalk.SpecFlow.Assist.Attributes;

namespace AlfaBank.AFT.Core.Model.Web
{
    public class Element
    {
        [TableAliases("name", "Имя", "Название")]
        public string Name { get; set; } 
        [TableAliases("ID", "id", "Идентификатор")]
        public string Id { get; set; }
        [TableAliases("XPath", "xpath")]
        public string Xpath { get; set; }
        [TableAliases("classname", "ClassName", "Класс")]
        public string Classname { get; set; }
        [TableAliases("tag", "TAG", "Тег")]
        public string Tag { get; set; }
    }
}
