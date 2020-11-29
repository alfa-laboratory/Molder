using FluentAssertions;
using EvidentInstruction.Controllers;
using EvidentInstruction.Generator.Steps;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace EvidentInstruction.Generator.Tests
{
    [ExcludeFromCodeCoverage]
    public class GeneratorStepsTests
    {
        private VariableController variableController;
        public GeneratorStepsTests()
        {
            variableController = new VariableController();
        }

        [Fact]
        public void StoreAsVariableDate_CorrectVariable_ReturnTrue()
        {
            GeneratorSteps steps = new GeneratorSteps(variableController);
            steps.StoreAsVariableDate(1, 1, 2000, "test");
            var dt = new DateTime(2000, 1, 1);

            var dateTime = variableController.GetVariableValue("test");
            dateTime.Should().Be(dt);
        }

        //[Fact]
        //public void StoreAsVariableDate_CorrectVariable_ReturnTrue()
        //{
        //    var variable = new Variable() { Type = typeof(string), Value = string.Empty };
        //    variableController.Variables.Add("test", variable);
        //
        //    VariableSteps steps = new VariableSteps(variableController);
        //    steps.DeleteVariable("test");
        //
        //    var check = variableController.CheckVariableByKey("test");
        //    check.Should().BeFalse();
        //}
    }
}
