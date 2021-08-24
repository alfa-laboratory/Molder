using FluentAssertions;
using Molder.Controllers;
using Molder.Generator.Steps;
using Molder.Helpers;
using Molder.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using TechTalk.SpecFlow;
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
                .WithMessage("Expected variableController.Variables {empty} to contain key \"test\" because переменная \"test\" не существует.");
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
                .WithMessage("Expected variableController.Variables {empty} to contain key \"test\" because переменная \"test\" не существует.");
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
                .WithMessage("Expected variableController.Variables {empty} to contain key \"test\" because переменная \"test\" не существует.");
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
                .WithMessage($"Expected value not to be <null> or whitespace because значение переменной \"test\" пустая строка, but found <null>.");
        }

        [Fact]
        public void CheckVariableIsNotEmpty_VariableValueIsEmpty_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableIsNotEmpty("test");
            act.Should().Throw<Exception>()
                .WithMessage($"Expected value not to be <null> or whitespace because значение переменной \"test\" пустая строка, but found \"\".");
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
        public void CheckVariableIsEmpty_VariableValueIsNull_ReturnNotException()
        {
            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableIsEmpty("test");
            act.Should().NotThrow<Exception>();
        }
        
        [Fact]
        public void CheckVariableIsEmpty_VariableValueWhiteSpace_ReturnNotException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "    " };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableIsEmpty("test");
            act.Should().NotThrow<Exception>();
        }

        [Fact]
        public void CheckVariableIsEmpty_VariableValueIsEmpty_ReturnException()
        {
            var variable = new Variable() { Type = typeof(string), Value = "test" };
            variableController.Variables.TryAdd("test", variable);

            VariableSteps steps = new VariableSteps(variableController);

            Action act = () => steps.CheckVariableIsEmpty("test");
            act.Should().Throw<Exception>()
                .WithMessage($"Expected value to be <null> or whitespace because значение переменной \"test\" не пустая строка, but found \"test\".");
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
            var variable = new Variable() { Type = typeof(string), Value = null };
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

        [Theory]
        [InlineData("8","qwerty")]
        [InlineData("99999999999999999", "5.23")]
        public void TransformationToEnumerable_Strings_ReturnTrue(string value1, string value2)
        {
            var table = new Table(new string[] { value1, value2 });
            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test", variable);
            var steps = new VariableSteps(variableController);
            var result = steps.TransformationTableToEnumerable(table);
            result.GetType().Name.Should().Be("List`1");
        }

        [Theory]
        [InlineData("8", "qwerty")]
        [InlineData("99999999999999999", "5.23")]
        public void TransformationToDictionary_Strings_ReturnTrue(string value1, string value2)
        {
            var table = new Table(new string[] { value1 });
            table.AddRow(value2);
            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test", variable);
            var steps = new VariableSteps(variableController);
            var result = steps.TransformationTableToDictionary(table);
            result[value1].Should().Be(value2);
            result.GetType().Name.Should().Be("Dictionary`2");
        }

        [Fact]
        public void ToTypeCode_WrongString_ReturnException()
        {
            var type = "qwerty";
            var steps = new VariableSteps(variableController);
            Action act = () => steps.StringToTypeCode(type);
            act.Should().Throw<Exception>()
                .WithMessage($"There is no type \"{type}\"");
        }

        [Fact]
        public void StringToTypeCode_NullType_ReturnException()
        {
            var steps = new VariableSteps(variableController);
            Action act = () => steps.StringToTypeCode(null);
            act.Should().Throw<Exception>()
                .WithMessage("Expected type not to be <null> because Значение \"type\" не задано.");
        }

        [Theory]
        [InlineData("int", "Int32")]
        [InlineData("bool", "Boolean")]
        [InlineData("long", "Int64")]
        [InlineData("double", "Double")]
        [InlineData("float", "Single")]
        [InlineData("string", "String")]
        [InlineData("object", "Object")]
        public void StringToTypeCode_Type_ReturnTrue(string value, string regType)
        {
            var steps = new VariableSteps(variableController);
            var res = steps.StringToTypeCode(value);
            res.ToString().Should().Be(regType);
        }

        [Fact]
        public void StoreEnumerableAsVariableNoType_NoVarName_ReturnException()
        {
            var table = new Table(new string[] { "test1", "test2" });
            var steps = new VariableSteps(variableController);
            var result = steps.TransformationTableToEnumerable(table);
            Action act = () => steps.StoreEnumerableAsVariableNoType(null, result);
            act.Should().Throw<Exception>()
                .WithMessage($"Expected varName not to be <null> because Значение \"varName\" не задано.");
        }

        [Theory]
        [InlineData("Test","8", "qwerty")]
        [InlineData("test","99999999999999999", "5.23")]
        [InlineData("543", "99999999999999999", "5.23")]
        public void StoreEnumerableAsVariableNoType_ReturnTrue(string varName, string value1, string value2)
        {
            var table = new Table(new string[] { value1, value2 });
            var steps = new VariableSteps(variableController);
            var res = steps.TransformationTableToEnumerable(table);
            steps.StoreEnumerableAsVariableNoType(varName, res);
            var result = variableController.GetVariableValue(varName);
            result.Should().NotBeNull();
        }

        [Theory]
        [InlineData("Test", "int", "5")]
        [InlineData("Test", "object", "5")]
        [InlineData("Test", "long", "5")]
        [InlineData("Test", "float", "5")]
        [InlineData("Test", "double", "5")]
        [InlineData("Test", "string", "test")]
        [InlineData("Test", "bool", "True")]
        public void StoreEnumerableAsVariableWithType_ReturnTrue(string varName, string type, string value)
        {
            var table = new Table(new string[] { value });
            var steps = new VariableSteps(variableController);
            var varType = steps.StringToTypeCode(type);
            var res = steps.TransformationTableToEnumerable(table);
            steps.StoreEnumerableAsVariableWithType(varType, varName, res);
            var result = variableController.GetVariableValue(varName);
            (result is IEnumerable).Should().BeTrue();
        }

        [Fact]
        public void StoreEnumerableAsVariableWithType_NoVarName_ReturnException()
        {
            var steps = new VariableSteps(variableController);
            var table = new Table(new string[] { "test" });
            var res = steps.TransformationTableToEnumerable(table);
            Action act = () => steps.StoreEnumerableAsVariableWithType(TypeCode.Int32, null, res);
            act.Should().Throw<Exception>()
                .WithMessage("Expected varName not to be <null> because Значение \"varName\" не задано.");
        }

        [Theory]
        [InlineData("int", 5,6)]
        [InlineData("object", 5,6)]
        [InlineData("long", 5L,6L)]
        [InlineData("float", 5.1F,6.2F)]
        [InlineData("double", 5.84,6.32)]
        [InlineData("string", "test","qwerty")]
        [InlineData("bool", true,false)]
        public void StoreRandomVariableFromEnumerable_ReturnTrue(string type, object value1, object value2)
        {
            var steps = new VariableSteps(variableController);
            var collectionName = "Test";
            var newVarName = "Test2";
            var varType = steps.StringToTypeCode(type);
            var collection = new List<object>() {value1,value2};
            steps.StoreEnumerableAsVariableWithType(varType, collectionName, collection);
            steps.StoreRandomVariableFromEnumerable(collectionName, newVarName);
            var result = variableController.GetVariableValue(newVarName);
            collection.Should().Contain(result);
        }

        [Fact]
        public void StoreRandomVariableFromEnumerable_NoCollName_ReturnException()
        {
            var steps = new VariableSteps(variableController);
            Action act = () => steps.StoreRandomVariableFromEnumerable(null, "Test2");
            act.Should().Throw<Exception>()
                .WithMessage("Expected collectionName not to be <null> because Значение \"collectionName\" не задано.");
        }

        [Fact]
        public void StoreRandomVariableFromEnumerable_NoVarName_ReturnException()
        {
            var steps = new VariableSteps(variableController);
            Action act = () => steps.StoreRandomVariableFromEnumerable("Test", null);
            act.Should().Throw<Exception>()
                .WithMessage("Expected varName not to be <null> because Значение \"varName\" не задано.");
        }

        [Fact]
        public void StoreRandomVariableFromEnumerable_NoColl_ReturnException()
        {
            var steps = new VariableSteps(variableController);
            Action act = () => steps.StoreRandomVariableFromEnumerable("Test", "Test2");
            act.Should().Throw<Exception>()
                .WithMessage("Expected collection not to be <null> because значения в переменной \"Test\" нет.");
        }

        [Fact]
        public void StoreRandomVariableFromEnumerable_NotColl_ReturnException()
        {
            var steps = new VariableSteps(variableController);
            var tmp = 5;
            variableController.SetVariable("Test", tmp.GetType(), tmp);
            Action act = () => steps.StoreRandomVariableFromEnumerable("Test", "Test2");
            act.Should().Throw<Exception>()
                .Where(e => e.Message.Contains("\"Test\" не является коллекцией"));
        }

        public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { new List<object> { 0, 1, 2, 3, 4, 5, 6 }, TypeCode.Int32, },
            new object[] { new List<object> { 0, 1, 2, 3, 4, 5, 6 }, TypeCode.Object },
            new object[] { new List<object> { 0, 1, 2, 3, 4, 5, 6 }, TypeCode.Int64 },
            new object[] { new List<object> { 0, 1, 2, 3, 4, 5, 6 }, TypeCode.Single },
            new object[] { new List<object> { 0.0, 1.1, 2.2, 3.3, 4.4, 5.5, 6.6 }, TypeCode.Double },
            new object[] { new List<object> { "0", "1", "2", "3", "4", "5", "6" }, TypeCode.String },
            new object[] { new List<object> { true, false }, TypeCode.Boolean }
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void StoreVariableFromEnumerable_ReturnTrue( List<object> collection, TypeCode type)
        {
            var expected = collection[1];
            var steps = new VariableSteps(variableController);
            steps.StoreEnumerableAsVariableWithType(type, "Test", collection);
            steps.StoreVariableFromEnumerable("Test[1]","Test2");
            var actual = variableController.GetVariableValue("Test2");
            expected.Should().BeEquivalentTo(actual);
        }

        [Fact]
        public void StoreVariableFromEnumerable_NoCollName_ReturnException()
        {
            var steps = new VariableSteps(variableController);
            Action act = () => steps.StoreVariableFromEnumerable(null, "Test2");
            act.Should().Throw<Exception>()
                .WithMessage("Expected collectionName not to be <null> because Значение \"collectionName\" не задано.");
        }

        [Fact]
        public void StoreVariableFromEnumerable_NoColl_ReturnException()
        {
            var steps = new VariableSteps(variableController);
            Action act = () => steps.StoreVariableFromEnumerable("Test", "Test2");
            act.Should().Throw<Exception>()
                .WithMessage("Expected collection not to be <null> because Значения в переменной \"Test\" нет.");
        }

        [Fact]
        public void StoreVariableFromEnumerable_NotColl_ReturnException()
        {
            var steps = new VariableSteps(variableController);
            var tmp = 5;
            variableController.SetVariable("Test", tmp.GetType(), tmp);
            Action act = () => steps.StoreVariableFromEnumerable("Test", "Test2");
            act.Should().Throw<Exception>()
                .Where(e => e.Message.Contains("\"Test\" не является коллекцией"));
        }

        [Fact]
        public void StoreVariableFromEnumerable_BigNumber_ReturnException()
        {
            var steps = new VariableSteps(variableController);
            var collection = new List<object>() { 5 };
            var varName = "Test[10]";
            steps.StoreEnumerableAsVariableWithType(TypeCode.Object, "Test", collection);
            Action act = () => steps.StoreVariableFromEnumerable(varName, "Test2");
            act.Should().Throw<Exception>()
                .WithMessage("Index was out of range. Must be non-negative and less than the size of the collection. (Parameter 'index')");
        }

        [Fact]
        public void StoreVariableFromEnumerable_BadNumber_ReturnException()
        {
            var steps = new VariableSteps(variableController);
            var collection = new List<object>() { 5 };
            var varName = "Test[qwerty]";
            steps.StoreEnumerableAsVariableWithType(TypeCode.Object, "Test", collection);
            Action act = () =>steps.StoreVariableFromEnumerable(varName, "Test2");
            act.Should().Throw<Exception>()
                .Where(e => e.Message.Contains("\"Test[qwerty]\" не является значением коллекции"));
        }

        [Fact]
        public void StoreVariableFromEnumerable_WhiteSpace_ReturnException()
        {
            var steps = new VariableSteps(variableController);
            var collection = new List<object>() { 5 };
            var varName = "Test[   ]";
            steps.StoreEnumerableAsVariableWithType(TypeCode.Object, "Test", collection);
            Action act = () => steps.StoreVariableFromEnumerable(varName, "Test2");
            act.Should().Throw<Exception>()
                .Where(e => e.Message.Contains("\"Test[   ]\" не является значением коллекции"));
        }

        [Fact]
        public void StoreDictionaryAsVariableNoType_ReturnException()
        {
            var steps = new VariableSteps(variableController);
            var table = new Table(new string[] { "Test" });
            table.AddRow("5");
            var dictionary = steps.TransformationTableToDictionary(table);
            Action act = () => steps.StoreDictionaryAsVariableNoType(null, dictionary);
            act.Should().Throw<Exception>()
                .WithMessage("Expected varName not to be <null> because Значение \"varname\" не задано.");
        }

        [Theory]
        [InlineData("string")]
        [InlineData("test")]
        [InlineData("845")]
        public void StoreDictionaryAsVariableNoType_ReturnTrue(string value)
        {
            var steps = new VariableSteps(variableController);
            var dictionary = new Dictionary<string, object>() { { "Test", value } };
            var varName = "Test";
            steps.StoreDictionaryAsVariableNoType(varName, dictionary);
            var result = variableController.GetVariableValue(varName);
            result.Should().Be(dictionary);
        }

        [Fact]
        public void StoreRandomVariableFromDictionary_NoDictName_ReturnException()
        {
            var steps = new VariableSteps(variableController);
            Action act = () => steps.StoreRandomVariableFromDictionary(null, "tmp");
            act.Should().Throw<Exception>()
                .WithMessage("Expected dictionaryName not to be <null> because Значение \"dictionaryName\" не задано.");
        }

        [Fact]
        public void StoreRandomVariableFromDictionary_NoVarName_ReturnException()
        {
            var steps = new VariableSteps(variableController);
            Action act = () => steps.StoreRandomVariableFromDictionary("Test", null);
            act.Should().Throw<Exception>()
                .WithMessage("Expected varName not to be <null> because Значение \"varname\" не задано.");
        }

        [Fact]
        public void StoreRandomVariableFromDictionary_NotDict_ReturnException()
        {
            var steps = new VariableSteps(variableController);
            var variable = 5;
            variableController.SetVariable("Test", variable.GetType(), variable);
           
            Action act = () => steps.StoreRandomVariableFromDictionary("Test", "Test2");
            act.Should().Throw<Exception>()
                .Where(e => e.Message.Contains("\"Test\" не является словарем"));
        }

        [Theory]
        [InlineData("string")]
        [InlineData("test")]
        [InlineData("845")]
        public void StoreRandomVariableFromDictionary_ReturnTrue(string value)
        {
            var steps = new VariableSteps(variableController);
            var dictionary = new Dictionary<string, object>() { { "Test", value } };
            var varName = "Test";
            var newVarName = "tmp";
            steps.StoreDictionaryAsVariableNoType(varName, dictionary);
            steps.StoreRandomVariableFromDictionary(varName, newVarName);
            var result = variableController.GetVariableValueText(newVarName);
            result.Should().Be(value);
        }

        [Fact]
        public void StoreVariableFromDictionary_NoDictName_ReturnException()
        {
            var steps = new VariableSteps(variableController);
            Action act = () => steps.StoreVariableFromDictionary(null, "Test2");
            act.Should().Throw<Exception>()
                .WithMessage("Expected dictionaryName not to be <null> because Значение \"dictionaryName\" не задано.");
        }

        [Fact]
        public void StoreVariableFromDictionary_NoKey_ReturnException()
        {
            var steps = new VariableSteps(variableController);
            var dictionary = new Dictionary<string, object>() { { "Test", "5" } };
            var varName = "Test";
            var newVarName = "tmp";
            steps.StoreDictionaryAsVariableNoType(varName, dictionary);
            Action act = () => steps.StoreVariableFromDictionary(varName, newVarName);
            act.Should().Throw<Exception>()
                .Where(e => e.Message.Contains("\"Test\" не является значением коллекции"));
        }

        [Fact]
        public void StoreVariableFromDictionary_NullKey_ReturnException()
        {
            var steps = new VariableSteps(variableController);
            var dictionary = new Dictionary<string, object>() { { "Test", "5" } };
            var varName = "Test";
            var newVarName = "tmp";
            steps.StoreDictionaryAsVariableNoType(varName, dictionary);
            Action act = () => steps.StoreVariableFromDictionary(varName+"[]", newVarName);
            act.Should().Throw<Exception>()
                .Where(e => e.Message.Contains("\"Test[]\" не является значением коллекции"));
        }

        [Fact]
        public void StoreVariableFromDictionary_WrongKey_ReturnException()
        {
            var steps = new VariableSteps(variableController);
            var dictionary = new Dictionary<string, object>() { { "Test", "5" } };
            var varName = "Test";
            var newVarName = "tmp";
            steps.StoreDictionaryAsVariableNoType(varName, dictionary);
            Action act = () => steps.StoreVariableFromDictionary(varName + "[qwerty]", newVarName);
            act.Should().Throw<Exception>()
                .WithMessage("The given key 'qwerty' was not present in the dictionary.");
        }

        [Fact]
        public void StoreVariableFromDictionary_WhiteSpaceKey_ReturnException()
        {
            var steps = new VariableSteps(variableController);
            var dictionary = new Dictionary<string, object>() { { "Test", "5" } };
            var varName = "Test";
            var newVarName = "tmp";
            steps.StoreDictionaryAsVariableNoType(varName, dictionary);
            Action act = () => steps.StoreVariableFromDictionary(varName + "[   ]", newVarName);
            act.Should().Throw<Exception>()
                .Where(e => e.Message.Contains("\"Test[   ]\" не является значением коллекции"));
        }

        [Fact]
        public void StoreVariableFromDictionary_NoVarName_ReturnException()
        {
            var steps = new VariableSteps(variableController);
            var dictionary = new Dictionary<string, object>() { { "Test", 5 } };
            var varName = "Test";
            steps.StoreDictionaryAsVariableNoType(varName, dictionary);
            Action act = () => steps.StoreVariableFromDictionary(varName, null);
            act.Should().Throw<Exception>()
                .WithMessage("Expected varName not to be <null> because Значение \"varName\" не задано.");
        }

        [Theory]
        [InlineData("string")]
        [InlineData("test")]
        [InlineData("845")]
        public void StoreVariableFromDictionary_ReturnTrue(string value)
        {
            var steps = new VariableSteps(variableController);
            var dictionary = new Dictionary<string, object>() { { "Test", value } };
            var varName = "Test";
            var newVarName = "tmp";
            steps.StoreDictionaryAsVariableNoType(varName, dictionary);
            steps.StoreVariableFromDictionary(varName+"[Test]", newVarName);
            var result = variableController.GetVariableValueText(newVarName);
            result.Should().Be(value);
        }
    }
}