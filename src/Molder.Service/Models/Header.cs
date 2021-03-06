﻿using Molder.Service.Infrastructures;
using TechTalk.SpecFlow.Assist.Attributes;

namespace Molder.Service.Models
{
    public class Header
    {
        [TableAliases("Name", "Имя")]
        public string Name { get; set; }

        [TableAliases("Value", "Значение")]
        public string Value { get; set; }

        [TableAliases("Style", "Тип")] 
        public HeaderType Style { get; set; } 
    }
}