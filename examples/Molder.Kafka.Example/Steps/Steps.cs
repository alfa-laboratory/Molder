using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Molder.Controllers;
using TechTalk.SpecFlow;
using Xunit.Abstractions;

namespace Molder.Kafka.Example.Steps
{
    [ExcludeFromCodeCoverage]
    [Binding]
    public class Steps
    {
        private VariableController variables;
        private ITestOutputHelper output;

        public Steps(VariableController variables, ITestOutputHelper output)
        {
            this.variables = variables;
            this.output = output;
        }

        [StepDefinition(@"write list from variable ""(.+)""")]
        public void Output(string varName)
        {
            variables.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            var list = variables.GetVariableValue(varName) as List<object>;
            foreach (string variable in list)
            {
                output.WriteLine($"Variable value is '{variable}'");
            }
            
        }
    }
}