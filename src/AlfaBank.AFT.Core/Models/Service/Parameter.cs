using AlfaBank.AFT.Core.Infrastructure.Service;
using TechTalk.SpecFlow.Assist.Attributes;

namespace AlfaBank.AFT.Core.Models.Service
{
    public class Parameter
    {
        [TableAliases("Name")]
        public string Name { get; set; }
        [TableAliases("Value")]
        public string Value { get; set; }
        [TableAliases("Style")]
        public ParameterType ParameterType { get; set; }
    }
}
