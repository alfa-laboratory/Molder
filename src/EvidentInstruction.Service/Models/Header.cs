using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow.Assist.Attributes;

namespace EvidentInstruction.Service.Models
{
    public class Header
    {
        [TableAliases("Name", "Имя")]
        public string Name { get; set; }

        [TableAliases("Value", "Значение")]
        public string Value { get; set; }
    }
}