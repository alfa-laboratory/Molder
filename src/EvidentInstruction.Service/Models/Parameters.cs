using TechTalk.SpecFlow.Assist.Attributes;

namespace EvidentInstruction.Service.Models
{
    public class Parameters
    {
        [TableAliases("Name", "Имя")]
        public string Name { get; set; }
        [TableAliases("Value", "Значение")]
        public string Value { get; set; }

        [TableAliases("Type", "Тип")]
        public string Type { get; set; }

    }
}
