using Molder.Controllers;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using TechTalk.SpecFlow;
using System;

namespace Molder.Configuration.Example.Steps
{
    [ExcludeFromCodeCoverage]
    [Binding]
    public class Steps
    {
        private VariableController variables;

        public Steps(VariableController variables)
        {
            this.variables = variables;
        }

        [StepDefinition(@"write variable ""(.+)""")]
        public void Output(string varName)
        {
            this.variables.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            var value = variables.GetVariableValueText(varName);
            Console.WriteLine($"Variable value is {value}");
        }
    }
}
