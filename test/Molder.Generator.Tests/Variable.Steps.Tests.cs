using FluentAssertions;
using Molder.Controllers;
using Molder.Generator.Steps;
using Molder.Helpers;
using Molder.Models;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using Xunit;

namespace Molder.Generator.Tests
{
    [ExcludeFromCodeCoverage]
    public class VariableStepsTests
    {
        private VariableController variableController;
        public VariableStepsTests()
        {
            variableController = new VariableController();
        }

        [Fact]
        public void DeleteVariable_CorrectVariable_ReturnFalse()
        {
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.DeleteVariable("test");

            var check = variableController.CheckVariableByKey("test");
            check.Should().BeFalse();
        }

        [Fact]
        public void DeleteVariable_CorrectVariable_ReturnException()
        {
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.DeleteVariable("test");
            act.Should().Throw<Exception>()
                .WithMessage("Expected this.variableController.Variables {empty} to contain key \"test\" because переменная \"test\" не существует.");
        }

        [Fact]
        public void EmtpyVariable_CorrectVariable_ReturnEmptyVariable()
        {
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.EmtpyVariable("test");

            var variableCheck = variableController.GetVariable("test");
            variableCheck.Type.Should().Be(typeof(object));
            variableCheck.Value.Should().BeNull();
        }

        [Fact]
        public void EmtpyVariable_CorrectVariable_ReturnException()
        {
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.EmtpyVariable("test");
            act.Should().Throw<Exception>()
                .WithMessage("Expected this.variableController.Variables {empty} to contain key \"test\" because переменная \"test\" не существует.");
        }

        [Fact]
        public void ChangeVariable_IntValue_ReturnNewVariable()
        {
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.ChangeVariable("test", 0);

            var variableCheck = variableController.GetVariable("test");
            variableCheck.Type.Should().Be(typeof(int));
            variableCheck.Value.Should().Be(0);
        }

        [Fact]
        public void ChangeVariable_StringValue_ReturnNewVariable()
        {
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.ChangeVariable("test", "test");

            var variableCheck = variableController.GetVariable("test");
            variableCheck.Type.Should().Be(typeof(string));
            variableCheck.Value.Should().Be("test");
        }

        [Fact]
        public void ChangeVariable_BoolValue_ReturnNewVariable()
        {
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.ChangeVariable("test", true);

            var variableCheck = variableController.GetVariable("test");
            variableCheck.Type.Should().Be(typeof(bool));
            variableCheck.Value.Should().Be(true);
        }

        [Fact]
        public void ChangeVariable_CorrectVariable_ReturnException()
        {
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.ChangeVariable("test", 0);
            act.Should().Throw<Exception>()
                .WithMessage("Expected this.variableController.Variables {empty} to contain key \"test\" because переменная \"test\" не существует.");
        }

        [Fact]
        public void StoreAsVariableString_ValidValue_ReturnNewVariable()
        {
            VariableSteps steps = new VariableSteps(variableController);
            steps.StoreAsVariableString("test", "test");

            var variableCheck = variableController.GetVariable("test");
            variableCheck.Type.Should().Be(typeof(string));
            variableCheck.Value.Should().Be("test");
        }

        [Fact]
        public void StoreAsVariableEncriptedString_ValidValue_ReturnNewVariable()
        {
            VariableSteps steps = new VariableSteps(variableController);
            var str = Encryptor.Encrypt("test");
            steps.StoreAsVariableEncriptedString(str, "test");

            var variableCheck = variableController.GetVariable("test");
            variableCheck.Type.Should().Be(typeof(string));
            variableCheck.Value.Should().Be("test");
        }

        [Fact]
        public void StoreAsVariableText_ValidValue_ReturnNewVariable()
        {
            VariableSteps steps = new VariableSteps(variableController);
            steps.StoreAsVariableText("test", "test");

            var variableCheck = variableController.GetVariable("test");
            variableCheck.Type.Should().Be(typeof(string));
            variableCheck.Value.Should().Be("test");
        }

        [Fact]
        public void StoreAsVariableNumber_IncorrectVariable_ReturnException()
        {
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.StoreAsVariableNumber("test", "test");
            act.Should().Throw<Exception>()
                .WithMessage($"Input string was not in a correct format.");
        }

        [Theory]
        [InlineData(
            "<?xml version=\"1.0\" encoding=\"UTF-8\"?><addresses xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation='test.xsd'><address><name>Joe Tester</name><street>Baker street</street><house>5</house></address></addresses>")]
        public void StoreAsVariableXmlFromText_CorrectXml_ReturnXmlDoc(string xml)
        {
            VariableSteps steps = new VariableSteps(variableController);
            steps.StoreAsVariableXmlFromText("test", xml);
            var variableCheck = variableController.GetVariable("test");
            variableCheck.Type.Should().Be(typeof(XmlDocument));
            variableCheck.Value.Should().NotBeNull();
        }

        [Fact]
        public void StoreAsVariableXmlFromText_IncorrectXml_ReturnException()
        {
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.StoreAsVariableXmlFromText("test", "test");
            act.Should().Throw<Exception>()
                .WithMessage($"Expected doc not to be <null> because создать XmlDoc из строки \"test\" не удалось.");
        }

        [Fact]
        public void StoreVariableValueToVariable_CorrectVariable_ReturnVariable()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.StoreVariableValueToVariable("test", "tmp");

            var variableCheck = variableController.GetVariable("tmp");
            variableCheck.Type.Should().Be(typeof(string));
            variableCheck.Value.Should().Be("test");
        }

        [Fact]
        public void StoreVariableValueToVariable_CorrectSecondVariable_ReturnException()
        {
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.StoreVariableValueToVariable("test", null);
            act.Should().Throw<Exception>()
                .WithMessage("Expected this.variableController.Variables {empty} to contain key \"test\" because переменная \"test\" не существует.");
        }

        [Fact]
        public void CheckVariableIsNotNull_CorrectVarName_ReturnTrue()
        {
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.CheckVariableIsNotNull("test");
        }

        [Fact]
        public void CheckVariableIsNotNull_VariableValueIsNull_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableIsNotNull("test");
            act.Should().Throw<Exception>()
                .WithMessage($"Expected value not to be <null> because значение переменной \"test\" является NULL.");
        }

        [Fact]
        public void CheckVariableIsNull_CorrectVarName_ReturnTrue()
        {
            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.CheckVariableIsNull("test");
        }

        [Fact]
        public void CheckVariableIsNull_VariableValueIsNull_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableIsNull("test");
            act.Should().Throw<Exception>()
                .WithMessage($"Expected value to be <null> because значение переменной \"test\" не является NULL, but found \"\".");
        }

        [Fact]
        public void CheckVariableIsNotEmpty_CorrectVarName_ReturnTrue()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.CheckVariableIsNotEmpty("test");
        }

        [Fact]
        public void CheckVariableIsNotEmpty_VariableValueIsNull_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableIsNotEmpty("test");
            act.Should().Throw<Exception>()
                .WithMessage($"Expected value not to be <null> because значения в переменной \"test\" нет.");
        }

        [Fact]
        public void CheckVariableIsNotEmpty_VariableValueIsEmpty_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableIsNotEmpty("test");
            act.Should().Throw<Exception>()
                .WithMessage($"Expected string.IsNullOrWhiteSpace((string)value) to be false because значение переменной \"test\" пустая строка, but found True.");
        }

        [Fact]
        public void CheckVariableIsEmpty_CorrectVarName_ReturnTrue()
        {
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.CheckVariableIsEmpty("test");
        }

        [Fact]
        public void CheckVariableIsEmpty_VariableValueIsNull_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableIsEmpty("test");
            act.Should().Throw<Exception>()
                .WithMessage($"Expected value not to be <null> because значения в переменной \"test\" нет.");
        }

        [Fact]
        public void CheckVariableIsEmpty_VariableValueIsEmpty_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableIsEmpty("test");
            act.Should().Throw<Exception>()
                .WithMessage($"Expected string.IsNullOrWhiteSpace((string)value) to be true because значение переменной \"test\" не пустая строка, but found False.");
        }

        [Fact]
        public void CheckVariableEquals_VariableValueIsValid_ReturnTrue()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.CheckVariableEquals("test", "test");
        }

        [Fact]
        public void CheckVariableEquals_InCorrectExpected_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableEquals("test", null);
            act.Should().Throw<Exception>()
                .WithMessage($"Expected expected not to be <null> because значение \"expected\" не задано.");
        }

        [Fact]
        public void CheckVariableEquals_InCorrectEquals_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableEquals("test", "mp");
            act.Should().Throw<Exception>()
                .WithMessage($"Expected expected to be \"test\" with a length of 4 because значение переменной \"test\":\"test\" не равно \"mp\", but \"mp\" has a length of 2, differs near \"mp\" (index 0).");
        }

        [Fact]
        public void CheckVariableNotEquals_VariableValueIsValid_ReturnTrue()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.CheckVariableNotEquals("test", "123");
        }

        [Fact]
        public void CheckVariableNotEquals_InCorrectExpected_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableNotEquals("test", null);
            act.Should().Throw<Exception>()
                .WithMessage($"Expected expected not to be <null> because значение \"expected\" не задано.");
        }

        [Fact]
        public void CheckVariableNotEquals_InCorrectEquals_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableNotEquals("test", "test");
            act.Should().Throw<Exception>()
                .WithMessage($"Expected expected not to be \"test\" because значение переменной \"test\":\"test\" равно \"test\".");
        }

        [Fact]
        public void CheckVariableContains_VariableValueIsValid_ReturnTrue()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.CheckVariableContains("test", "test");
        }

        [Fact]
        public void CheckVariableContains_InCorrectExpected_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableContains("test", null);
            act.Should().Throw<Exception>()
                .WithMessage($"Expected expected not to be <null> because значение \"expected\" не задано.");
        }

        [Fact]
        public void CheckVariableContains_InCorrectActual_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableContains("test", "test");
            act.Should().Throw<Exception>()
                .WithMessage($"Expected actual not to be <null> because значения в переменной \"test\" нет.");
        }

        [Fact]
        public void CheckVariableContains_InCorrectEquals_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableContains("test", "mp");
            act.Should().Throw<Exception>()
                .WithMessage($"Expected actual \"test\" to contain \"mp\" because значение переменной \"test\":\"test\" не содержит \"mp\".");
        }

        [Fact]
        public void CheckVariableNotContains_VariableValueIsValid_ReturnTrue()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.CheckVariableNotContains("test", "123");
        }

        [Fact]
        public void CheckVariableNotContains_InCorrectExpected_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableNotContains("test", null);
            act.Should().Throw<Exception>()
                .WithMessage($"Expected expected not to be <null> because значение \"expected\" не задано.");
        }

        [Fact]
        public void CheckVariableNotContains_InCorrectActual_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableNotContains("test", "test");
            act.Should().Throw<Exception>()
                .WithMessage($"Expected actual not to be <null> because значения в переменной \"test\" нет.");
        }

        [Fact]
        public void CheckVariableNotContains_InCorrectEquals_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableNotContains("test", "test");
            act.Should().Throw<Exception>()
                .WithMessage($"Did not expect actual \"test\" to contain \"test\" because значение переменной \"test\":\"test\" содержит \"test\".");
        }

        [Fact]
        public void CheckVariableStartsWith_VariableValueIsValid_ReturnTrue()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.CheckVariableStartsWith("test", "test");
        }

        [Fact]
        public void CheckVariableStartsWith_InCorrectExpected_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableStartsWith("test", null);
            act.Should().Throw<Exception>()
                .WithMessage($"Expected expected not to be <null> because значение \"expected\" не задано.");
        }

        [Fact]
        public void CheckVariableStartsWith_InCorrectActual_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = null};
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableStartsWith("test", "test");
            act.Should().Throw<Exception>()
                .WithMessage($"Expected actual not to be <null> because значения в переменной \"test\" нет.");
        }

        [Fact]
        public void CheckVariableStartsWith_InCorrectEquals_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableStartsWith("test", "mp");
            act.Should().Throw<Exception>()
                .WithMessage($"Expected actual to start with \"mp\" because значение переменной \"test\":\"test\" не начинается с \"mp\", but \"test\" differs near \"tes\" (index 0).");
        }

        [Fact]
        public void CheckVariableEndsWith_VariableValueIsValid_ReturnTrue()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.CheckVariableEndsWith("test", "test");
        }

        [Fact]
        public void CheckVariableEndsWith_InCorrectExpected_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableEndsWith("test", null);
            act.Should().Throw<Exception>()
                .WithMessage($"Expected expected not to be <null> because значение \"expected\" не задано.");
        }

        [Fact]
        public void CheckVariableEndsWith_InCorrectActual_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableEndsWith("test", "123");
            act.Should().Throw<Exception>()
                .WithMessage($"Expected actual not to be <null> because значения в переменной \"test\" нет.");
        }

        [Fact]
        public void CheckVariableEndsWith_InCorrectEquals_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableEndsWith("test", "123");
            act.Should().Throw<Exception>()
                .WithMessage($"Expected actual \"test\" to end with \"123\" because значение переменной \"test\":\"test\" не заканчивается с \"123\".");
        }

        [Fact]
        public void CheckVariablesAreEqual_VariableValueIsValid_ReturnTrue()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test1", variable);
            variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test2", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.CheckVariableEquals("test1", "{{test2}}");
        }

        [Fact]
        public void CheckVariablesAreEqual_InCorrectValue1_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test1", variable);
            variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test2", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableEquals("test1", "test2");
            act.Should().Throw<Exception>()
                .WithMessage($"Expected actual not to be <null> because значения в переменной \"test1\" нет.");
        }

        [Fact]
        public void CheckVariablesAreEqual_InCorrectActual_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "1" };
            variableController.Variables.TryAdd("test1", variable);
            variable = new Variable() { Type = typeof(string), Value = "2" };
            variableController.Variables.TryAdd("test2", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableEquals("test1", "{{test2}}");
            act.Should().Throw<Exception>()
                .WithMessage($"Expected expected to be \"1\" because значение переменной \"test1\":\"1\" не равно \"2\", but \"2\" differs near \"2\" (index 0).");
        }

        [Fact]
        public void CheckVariablesAreNotEqual_VariableValueIsValid_ReturnTrue()
        {
            var variable = new Variable() { Type = typeof(string), Value = "1" };
            variableController.Variables.TryAdd("test1", variable);
            variable = new Variable() { Type = typeof(string), Value = "2" };
            variableController.Variables.TryAdd("test2", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.CheckVariableNotEquals("test1", "{test2}");
        }

        [Fact]
        public void CheckVariablesAreNotEqual_InCorrectValue1_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test1", variable);
            variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test2", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableNotEquals("test1", "test2");
            act.Should().Throw<Exception>()
                .WithMessage($"Expected actual not to be <null> because значения в переменной \"test1\" нет.");
        }

        [Fact]
        public void CheckVariablesAreNotEqual_InCorrectActual_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "1" };
            variableController.Variables.TryAdd("test1", variable);
            variable = new Variable() { Type = typeof(string), Value = "1" };
            variableController.Variables.TryAdd("test2", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableNotEquals("test1", "{{test2}}");
            act.Should().Throw<Exception>()
                .WithMessage($"Expected expected not to be \"1\" because значение переменной \"test1\":\"1\" равно \"1\".");
        }

        [Fact]
        public void CheckVariableAreContains_VariableValueIsValid_ReturnTrue()
        {
            var variable = new Variable() { Type = typeof(string), Value = "1" };
            variableController.Variables.TryAdd("test1", variable);
            variable = new Variable() { Type = typeof(string), Value = "1" };
            variableController.Variables.TryAdd("test2", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.CheckVariableContains("test1", "{{test2}}");
        }

        [Fact]
        public void CheckVariableAreContains_InCorrectValue1_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test1", variable);
            variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test2", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableContains("test1", "test2");
            act.Should().Throw<Exception>()
                .WithMessage($"Expected actual not to be <null> because значения в переменной \"test1\" нет.");
        }

        [Fact]
        public void CheckVariableAreContains_InCorrectActual_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "1" };
            variableController.Variables.TryAdd("test1", variable);
            variable = new Variable() { Type = typeof(string), Value = "2" };
            variableController.Variables.TryAdd("test2", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableContains("test1", "{{test2}}");
            act.Should().Throw<Exception>()
                .WithMessage($"Expected actual \"1\" to contain \"2\" because значение переменной \"test1\":\"1\" не содержит \"2\".");
        }

        [Fact]
        public void CheckVariableAreNotContains_VariableValueIsValid_ReturnTrue()
        {
            var variable = new Variable() { Type = typeof(string), Value = "1" };
            variableController.Variables.TryAdd("test1", variable);
            variable = new Variable() { Type = typeof(string), Value = "2" };
            variableController.Variables.TryAdd("test2", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.CheckVariableNotContains("test1", "{test2}");
        }

        [Fact]
        public void CheckVariableAreNotContains_InCorrectValue1_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test1", variable);
            variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test2", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableNotContains("test1", "test2");
            act.Should().Throw<Exception>()
                .WithMessage($"Expected actual not to be <null> because значения в переменной \"test1\" нет.");
        }

        [Fact]
        public void CheckVariableAreNotContains_InCorrectActual_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "1" };
            variableController.Variables.TryAdd("test1", variable);
            variable = new Variable() { Type = typeof(string), Value = "1" };
            variableController.Variables.TryAdd("test2", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableNotContains("test1", "{{test2}}");
            act.Should().Throw<Exception>()
                .WithMessage($"Did not expect actual \"1\" to contain \"1\" because значение переменной \"test1\":\"1\" содержит \"1\".");
        }

        [Theory]
        [InlineData("a {test} c", 1, "a 1 c"), InlineData("a {test} c", "d", "a d c")]
        public void StoreAsVariableStringFormat_VariableValueIsValid_ReturnTrue(string text, object value, string res)
        {
            // Act 
            var variable = new Variable() { Type = value.GetType(), Value = value };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);

            // Arrange
            steps.StoreAsVariableStringFormat("test", text, "test2");

            var expected = variableController.Variables["test2"].Value;
            expected.Should().Be(res);
        }

        [Fact]
        public void StoreAsVariableStringFormat_IncorrectVariableName_ReturnException()
        {
            // Act
            VariableSteps steps = new VariableSteps(variableController);

            // Arrange
            Action act = () => steps.StoreAsVariableStringFormat("test", "text", "test2");

            // Assert
            act
              .Should().Throw<Exception>()
              .WithMessage("Expected this.variableController.Variables {empty} to contain key \"test\" because переменная \"test\" не существует.");
        }
    }
}