using FluentAssertions;
using EvidentInstruction.Controllers;
using EvidentInstruction.Generator.Steps;
using EvidentInstruction.Helpers;
using EvidentInstruction.Models;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using Xunit;

namespace EvidentInstruction.Generator.Tests
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
                .WithMessage($"*Переменной \"test\" не существует, but found False.*");
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
                .WithMessage($"*Переменной \"test\" не существует, but found False.*");
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
                .WithMessage($"*Переменной \"test\" не существует, but found False.*");
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
        public void StoreAsVariableString_CorrectVariable_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.StoreAsVariableString(null, "test");
            act.Should().Throw<Exception>()
                .WithMessage($"*Переменная \"test\" уже существует*");
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
        public void StoreAsVariableEncriptedString_CorrectVariable_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.StoreAsVariableEncriptedString(null, "test");
            act.Should().Throw<Exception>()
                .WithMessage($"*Переменная \"test\" уже существует*");
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
        public void StoreAsVariableText_CorrectVariable_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.StoreAsVariableText("test", null);
            act.Should().Throw<Exception>()
                .WithMessage($"*Переменная \"test\" уже существует*");
        }

        [Theory]
        [InlineData("0", 0), InlineData("0,1", 0.1), InlineData("0.1", 0.1)]
        public void StoreAsVariableNumber_IntValue_ReturnNewVariable(string number, object validNumber)
        {
            VariableSteps steps = new VariableSteps(variableController);
            steps.StoreAsVariableNumber("test", number);

            var variableCheck = variableController.GetVariable("test");
            variableCheck.Type.Should().Be(typeof(decimal));
            variableCheck.Value.Should().Be(validNumber);
        }

        [Fact]
        public void StoreAsVariableNumber_CorrectVariable_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.StoreAsVariableNumber("test", null);
            act.Should().Throw<Exception>()
                .WithMessage($"*Переменная \"test\" уже существует*");
        }

        [Fact]
        public void StoreAsVariableNumber_IncorrectVariable_ReturnException()
        {
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.StoreAsVariableNumber("test", "test");
            act.Should().Throw<Exception>()
                .WithMessage($"*Input string was not in a correct format*");
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
        public void StoreAsVariableXmlFromText_CorrectVariable_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.StoreAsVariableXmlFromText("test", null);
            act.Should().Throw<Exception>()
                .WithMessage($"*Переменная \"test\" уже существует*");
        }

        [Fact]
        public void StoreAsVariableXmlFromText_IncorrectXml_ReturnException()
        {
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.StoreAsVariableXmlFromText("test", "test");
            act.Should().Throw<Exception>()
                .WithMessage($"*Expected doc not to be <null>.");
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
        public void StoreVariableValueToVariable_CorrectFirstVariable_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.StoreVariableValueToVariable("test", "test");
            act.Should().Throw<Exception>()
                .WithMessage($"*Переменная \"test\" уже существует*");
        }

        [Fact]
        public void StoreVariableValueToVariable_CorrectSecondVariable_ReturnException()
        {
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.StoreVariableValueToVariable("test", null);
            act.Should().Throw<Exception>()
                .WithMessage($"*Переменной \"test\" не существует*");
        }

        [Fact]
        public void CheckVariableIsNotNull_CorrectVarName_ReturnTrue()
        {
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.CheckVariableIsNotNull("test");
        }

        [Theory]
        [InlineData(null), InlineData("")]
        public void CheckVariableIsNotNull_EmptyVarName_ReturnException(string varName)
        {
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableIsNotNull(varName);
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение \"varName\" не задано.*");
        }

        [Fact]
        public void CheckVariableIsNotNull_VariableValueIsNull_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableIsNotNull("test");
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение переменной \"test\" является NULL*");
        }

        [Fact]
        public void CheckVariableIsNull_CorrectVarName_ReturnTrue()
        {
            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.CheckVariableIsNull("test");
        }

        [Theory]
        [InlineData(null), InlineData("")]
        public void CheckVariableIsNull_EmptyVarName_ReturnException(string varName)
        {
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableIsNotNull(varName);
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение \"varName\" не задано.*");
        }

        [Fact]
        public void CheckVariableIsNull_VariableValueIsNull_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableIsNull("test");
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение переменной \"test\" не является NULL*");
        }

        [Fact]
        public void CheckVariableIsNotEmpty_CorrectVarName_ReturnTrue()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.CheckVariableIsNotEmpty("test");
        }

        [Theory]
        [InlineData(null), InlineData("")]
        public void CheckVariableIsNotEmpty_InCorrectVarName_ReturnException(string varName)
        {
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableIsNotEmpty(varName);
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение \"varName\" не задано.*");
        }

        [Fact]
        public void CheckVariableIsNotEmpty_VariableValueIsNull_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableIsNotEmpty("test");
            act.Should().Throw<Exception>()
                .WithMessage($"*Значения в переменной \"test\" нет*");
        }

        [Fact]
        public void CheckVariableIsNotEmpty_VariableValueIsEmpty_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableIsNotEmpty("test");
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение переменной \"test\" пустая строка*");
        }

        [Fact]
        public void CheckVariableIsEmpty_CorrectVarName_ReturnTrue()
        {
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.CheckVariableIsEmpty("test");
        }

        [Theory]
        [InlineData(null), InlineData("")]
        public void CheckVariableIsEmpty_InCorrectVarName_ReturnException(string varName)
        {
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableIsEmpty(varName);
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение \"varName\" не задано.*");
        }

        [Fact]
        public void CheckVariableIsEmpty_VariableValueIsNull_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableIsEmpty("test");
            act.Should().Throw<Exception>()
                .WithMessage($"*Значения в переменной \"test\" нет*");
        }

        [Fact]
        public void CheckVariableIsEmpty_VariableValueIsEmpty_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableIsEmpty("test");
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение переменной \"test\" не пустая строка*");
        }

        [Fact]
        public void CheckVariableEquals_VariableValueIsValid_ReturnTrue()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.CheckVariableEquals("test", "test");
        }

        [Theory]
        [InlineData(null), InlineData("")]
        public void CheckVariableEquals_InCorrectVarName_ReturnException(string varName)
        {
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableEquals(varName, null);
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение \"varName\" не задано.*");
        }

        [Fact]
        public void CheckVariableEquals_InCorrectExpected_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableEquals("test", null);
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение \"expected\" не задано.*");
        }

        [Fact]
        public void CheckVariableEquals_InCorrectType_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableEquals("test", 0);
            act.Should().Throw<Exception>()
                .WithMessage($"*Тип значения переменной \"test\" не совпадает с типом \"0\"*");
        }

        [Fact]
        public void CheckVariableEquals_InCorrectEquals_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableEquals("test", "mp");
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение переменной \"test\":\"test\" не равно \"mp\"*");
        }

        [Fact]
        public void CheckVariableNotEquals_VariableValueIsValid_ReturnTrue()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.CheckVariableNotEquals("test", "123");
        }

        [Theory]
        [InlineData(null), InlineData("")]
        public void CheckVariableNotEquals_InCorrectVarName_ReturnException(string varName)
        {
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableNotEquals(varName, null);
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение \"varName\" не задано.*");
        }

        [Fact]
        public void CheckVariableNotEquals_InCorrectExpected_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableNotEquals("test", null);
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение \"expected\" не задано.*");
        }

        [Fact]
        public void CheckVariableNotEquals_InCorrectType_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableNotEquals("test", 0);
            act.Should().Throw<Exception>()
                .WithMessage($"*Тип значения переменной \"test\" не совпадает с типом \"0\"*");
        }

        [Fact]
        public void CheckVariableNotEquals_InCorrectEquals_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableNotEquals("test", "test");
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение переменной \"test\":\"test\" равно \"test\"*");
        }

        [Fact]
        public void CheckVariableContains_VariableValueIsValid_ReturnTrue()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.CheckVariableContains("test", "test");
        }

        [Theory]
        [InlineData(null), InlineData("")]
        public void CheckVariableContains_InCorrectVarName_ReturnException(string varName)
        {
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableContains(varName, null);
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение \"varName\" не задано.*");
        }

        [Fact]
        public void CheckVariableContains_InCorrectExpected_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableContains("test", null);
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение \"expected\" не задано.*");
        }

        [Fact]
        public void CheckVariableContains_InCorrectActual_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableContains("test", "test");
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение переменной \"test\" NULL.*");
        }

        [Fact]
        public void CheckVariableContains_InCorrectEquals_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableContains("test", "mp");
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение переменной \"test\":\"test\" не содержит \"mp\"*");
        }

        [Fact]
        public void CheckVariableNotContains_VariableValueIsValid_ReturnTrue()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.CheckVariableNotContains("test", "123");
        }

        [Theory]
        [InlineData(null), InlineData("")]
        public void CheckVariableNotContains_InCorrectVarName_ReturnException(string varName)
        {
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableNotContains(varName, null);
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение \"varName\" не задано.*");
        }

        [Fact]
        public void CheckVariableNotContains_InCorrectExpected_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableNotContains("test", null);
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение \"expected\" не задано.*");
        }

        [Fact]
        public void CheckVariableNotContains_InCorrectActual_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableNotContains("test", "test");
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение переменной \"test\" NULL.*");
        }

        [Fact]
        public void CheckVariableNotContains_InCorrectEquals_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableNotContains("test", "test");
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение переменной \"test\":\"test\" содержит \"test\"*");
        }

        [Fact]
        public void CheckVariableStartsWith_VariableValueIsValid_ReturnTrue()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.CheckVariableStartsWith("test", "test");
        }

        [Theory]
        [InlineData(null), InlineData("")]
        public void CheckVariableStartsWith_InCorrectVarName_ReturnException(string varName)
        {
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableStartsWith(varName, null);
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение \"varName\" не задано.*");
        }

        [Fact]
        public void CheckVariableStartsWith_InCorrectExpected_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableStartsWith("test", null);
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение \"expected\" не задано.*");
        }

        [Fact]
        public void CheckVariableStartsWith_InCorrectActual_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = null};
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableStartsWith("test", "test");
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение переменной \"test\" NULL.*");
        }

        [Fact]
        public void CheckVariableStartsWith_InCorrectEquals_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableStartsWith("test", "mp");
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение переменной \"test\":\"test\" не начинается с \"mp\"*");
        }

        [Fact]
        public void CheckVariableEndsWith_VariableValueIsValid_ReturnTrue()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.CheckVariableEndsWith("test", "test");
        }

        [Theory]
        [InlineData(null), InlineData("")]
        public void CheckVariableEndsWith_InCorrectVarName_ReturnException(string varName)
        {
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableEndsWith(varName, null);
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение \"varName\" не задано.*");
        }

        [Fact]
        public void CheckVariableEndsWith_InCorrectExpected_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableEndsWith("test", null);
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение \"expected\" не задано.*");
        }

        [Fact]
        public void CheckVariableEndsWith_InCorrectActual_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableEndsWith("test", "123");
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение переменной \"test\" NULL.*");
        }

        [Fact]
        public void CheckVariableEndsWith_InCorrectEquals_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableEndsWith("test", "123");
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение переменной \"test\":\"test\" не заканчивается с \"123\"*");
        }

        [Fact]
        public void CheckVariablesAreEqual_VariableValueIsValid_ReturnTrue()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test1", variable);
            variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test2", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.CheckVariablesAreEqual("test1", "test2");
        }

        [Theory]
        [InlineData(null), InlineData("")]
        public void CheckVariablesAreEqual_InCorrectVarName1_ReturnException(string varName)
        {
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariablesAreEqual(varName, null);
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение \"varName1\" не задано.*");
        }

        [Theory]
        [InlineData(null), InlineData("")]
        public void CheckVariablesAreEqual_InCorrectVarName2_ReturnException(string varName)
        {
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariablesAreEqual("test", varName);
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение \"varName2\" не задано.*");
        }

        [Fact]
        public void CheckVariablesAreEqual_InCorrectValue1_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test1", variable);
            variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test2", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariablesAreEqual("test1", "test2");
            act.Should().Throw<Exception>()
                .WithMessage($"*Значения в переменной \"test1\" нет*");
        }

        [Fact]
        public void CheckVariablesAreEqual_InCorrectValue2_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test1", variable);
            variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test2", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariablesAreEqual("test1", "test2");
            act.Should().Throw<Exception>()
                .WithMessage($"*Значения в переменной \"test2\" нет*");
        }

        [Fact]
        public void CheckVariablesAreEqual_InCorrectActual_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "1" };
            variableController.Variables.TryAdd("test1", variable);
            variable = new Variable() { Type = typeof(string), Value = "2" };
            variableController.Variables.TryAdd("test2", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariablesAreEqual("test1", "test2");
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение переменной \"test1\":\"1\" не равно значению переменной \"test2\":\"2\"*");
        }

        [Fact]
        public void CheckVariablesAreNotEqual_VariableValueIsValid_ReturnTrue()
        {
            var variable = new Variable() { Type = typeof(string), Value = "1" };
            variableController.Variables.TryAdd("test1", variable);
            variable = new Variable() { Type = typeof(string), Value = "2" };
            variableController.Variables.TryAdd("test2", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.CheckVariablesAreNotEqual("test1", "test2");
        }

        [Theory]
        [InlineData(null), InlineData("")]
        public void CheckVariablesAreNotEqual_InCorrectVarName1_ReturnException(string varName)
        {
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariablesAreEqual(varName, null);
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение \"varName1\" не задано.*");
        }

        [Theory]
        [InlineData(null), InlineData("")]
        public void CheckVariablesAreNotEqual_InCorrectVarName2_ReturnException(string varName)
        {
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariablesAreEqual("test", varName);
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение \"varName2\" не задано.*");
        }

        [Fact]
        public void CheckVariablesAreNotEqual_InCorrectValue1_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test1", variable);
            variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test2", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariablesAreEqual("test1", "test2");
            act.Should().Throw<Exception>()
                .WithMessage($"*Значения в переменной \"test1\" нет*");
        }

        [Fact]
        public void CheckVariablesAreNotEqual_InCorrectValue2_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test1", variable);
            variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test2", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariablesAreEqual("test1", "test2");
            act.Should().Throw<Exception>()
                .WithMessage($"*Значения в переменной \"test2\" нет*");
        }

        [Fact]
        public void CheckVariablesAreNotEqual_InCorrectActual_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "1" };
            variableController.Variables.TryAdd("test1", variable);
            variable = new Variable() { Type = typeof(string), Value = "1" };
            variableController.Variables.TryAdd("test2", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariablesAreNotEqual("test1", "test2");
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение переменной \"test1\":\"1\" равно значению переменной \"test2\":\"1\"*");
        }

        [Fact]
        public void CheckVariableAreContains_VariableValueIsValid_ReturnTrue()
        {
            var variable = new Variable() { Type = typeof(string), Value = "1" };
            variableController.Variables.TryAdd("test1", variable);
            variable = new Variable() { Type = typeof(string), Value = "1" };
            variableController.Variables.TryAdd("test2", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.CheckVariableAreContains("test1", "test2");
        }

        [Theory]
        [InlineData(null), InlineData("")]
        public void CheckVariableAreContains_InCorrectVarName1_ReturnException(string varName)
        {
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableAreContains(varName, null);
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение \"varName1\" не задано.*");
        }

        [Theory]
        [InlineData(null), InlineData("")]
        public void CheckVariableAreContains_InCorrectVarName2_ReturnException(string varName)
        {
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableAreContains("test", varName);
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение \"varName2\" не задано.*");
        }

        [Fact]
        public void CheckVariableAreContains_InCorrectValue1_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test1", variable);
            variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test2", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableAreContains("test1", "test2");
            act.Should().Throw<Exception>()
                .WithMessage($"*Значения в переменной \"test1\" нет*");
        }

        [Fact]
        public void CheckVariableAreContains_InCorrectValue2_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test1", variable);
            variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test2", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableAreContains("test1", "test2");
            act.Should().Throw<Exception>()
                .WithMessage($"*Значения в переменной \"test2\" нет*");
        }

        [Fact]
        public void CheckVariableAreContains_InCorrectActual_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "1" };
            variableController.Variables.TryAdd("test1", variable);
            variable = new Variable() { Type = typeof(string), Value = "2" };
            variableController.Variables.TryAdd("test2", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableAreContains("test1", "test2");
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение переменной \"test1\":\"1\" не содержит значение переменной \"test2\":\"2\"*");
        }

        [Fact]
        public void CheckVariableAreNotContains_VariableValueIsValid_ReturnTrue()
        {
            var variable = new Variable() { Type = typeof(string), Value = "1" };
            variableController.Variables.TryAdd("test1", variable);
            variable = new Variable() { Type = typeof(string), Value = "2" };
            variableController.Variables.TryAdd("test2", variable);

            VariableSteps steps = new VariableSteps(variableController);
            steps.CheckVariableAreNotContains("test1", "test2");
        }

        [Theory]
        [InlineData(null), InlineData("")]
        public void CheckVariableAreNotContains_InCorrectVarName1_ReturnException(string varName)
        {
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableAreNotContains(varName, null);
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение \"varName1\" не задано.*");
        }

        [Theory]
        [InlineData(null), InlineData("")]
        public void CheckVariableAreNotContains_InCorrectVarName2_ReturnException(string varName)
        {
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableAreNotContains("test", varName);
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение \"varName2\" не задано.*");
        }

        [Fact]
        public void CheckVariableAreNotContains_InCorrectValue1_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test1", variable);
            variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test2", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableAreNotContains("test1", "test2");
            act.Should().Throw<Exception>()
                .WithMessage($"*Значения в переменной \"test1\" нет*");
        }

        [Fact]
        public void CheckVariableAreNotContains_InCorrectValue2_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test1", variable);
            variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test2", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableAreNotContains("test1", "test2");
            act.Should().Throw<Exception>()
                .WithMessage($"*Значения в переменной \"test2\" нет*");
        }

        [Fact]
        public void CheckVariableAreNotContains_InCorrectActual_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "1" };
            variableController.Variables.TryAdd("test1", variable);
            variable = new Variable() { Type = typeof(string), Value = "1" };
            variableController.Variables.TryAdd("test2", variable);
            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableAreNotContains("test1", "test2");
            act.Should().Throw<Exception>()
                .WithMessage($"*Значение переменной \"test1\":\"1\" содержит значение переменной \"test2\":\"1\"*");
        }
    }
}