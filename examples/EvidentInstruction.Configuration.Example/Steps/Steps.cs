using EvidentInstruction.Controllers;
using FluentAssertions;
using TechTalk.SpecFlow;
using Xunit.Abstractions;

namespace EvidentInstruction.Configuration.Example.Steps
{
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

        [StepDefinition(@"write variable ""(.+)""")]
        public void Output(string varName)
        {
            this.variables.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            var value = variables.GetVariableValueText(varName);
            output.WriteLine($"Variable value is {value}");
        }
    }
}
