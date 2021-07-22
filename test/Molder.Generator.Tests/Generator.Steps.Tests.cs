using FluentAssertions;
using Molder.Controllers;
using Molder.Generator.Steps;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using TechTalk.SpecFlow;
using Molder.Models;
using Moq;
using Molder.Generator.Models.Generators;
using Molder.Generator.Infrastructures;
using System.Collections.Generic;
using System.Net;
using Molder.Models.DateTimeHelpers;

namespace Molder.Generator.Tests
{
    [ExcludeFromCodeCoverage]
    public class GeneratorStepsTests
    {
        private const string _prepostfix = "_test_";

        private VariableController variableController;
        private FeatureContext featureContext = FeatureContext.Current;
        public GeneratorStepsTests()
        {
            variableController = new VariableController();

        }

        # region StoreAsVariableDate
        [Fact]
        public void StoreAsVariableDate_CorrectVariable_ReturnTrue()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);
            var dt = new DateTime(2000, 1, 1);

            // Arrange
            steps.StoreAsVariableDate(1, 1, 2000, "test");
           
            // Assert
            var dateTime = variableController.GetVariableValue("test");
            dateTime.Should().Be(dt);
        }

        [Fact]
        public void StoreAsVariableDate_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableDate(1, 1, 2000, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }

        [Fact]
        public void StoreAsVariableDate_IncorrectDate_ReturnException()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange

            Action act = () => steps.StoreAsVariableDate(0, 0, 0, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("проверьте корректность создания даты day:0,month:0,year:0");
        }

        [Fact]
        public void StoreAsVariableDateWithFormat_CorrectVariable_ReturnTrue()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);
            var dt = new DateTime(2000, 1, 1).ToString("yyyy-MM-dd");

            // Arrange
            steps.StoreAsVariableDateWithFormat(1, 1, 2000, "yyyy-MM-dd", "test");

            // Assert
            var dateTime = variableController.GetVariableValue("test");
            dateTime.Should().Be(dt);
        }

        [Fact]
        public void StoreAsVariableDateWithFormat_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableDateWithFormat(1, 1, 2000, "yyyy-MM-dd", "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }

        [Fact]
        public void StoreAsVariableDateWithFormat_IncorrectDate_ReturnException()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableDateWithFormat(0, 0, 0, "yyyy-MM-dd", "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("проверьте корректность создания даты day:0,month:0,year:0");
        }
#endregion
        # region StoreAsVariableTimeLong
                [Fact]
                public void StoreAsVariableTimeLong_CorrectVariable_ReturnTrue()
                {
                    // Act
                    var steps = new GeneratorSteps(variableController, featureContext);
                    var dt = new DateTime(1, 1, 1, 1, 1, 1, 0);

                    // Arrange
                    steps.StoreAsVariableTimeLong(1, 1, 1, 0, "test");

                    // Assert
                    var dateTime = variableController.GetVariableValue("test");
                    dateTime.Should().Be(dt);
                }

                [Fact]
                public void StoreAsVariableTimeLong_IncorrectVariableName_ReturnException()
                {
                    // Act
                    var variable = new Variable() { Type = typeof(string), Value = string.Empty };
                    variableController.Variables.TryAdd("test", variable);

                    var steps = new GeneratorSteps(variableController, featureContext);

                    // Arrange
                    Action act = () => steps.StoreAsVariableTimeLong(1, 1, 1, 0, "test");

                    // Assert
                    act
                      .Should().Throw<Exception>()
                      .Which.Message.Contains("переменная \"test\" уже существует");
                }

                [Fact]
                public void StoreAsVariableTimeLong_IncorrectDate_ReturnException()
                {
                    // Act
                    var steps = new GeneratorSteps(variableController, featureContext);

                    // Arrange

                    Action act = () => steps.StoreAsVariableTimeLong(-1, 0, 0, 0, "test");

                    // Assert
                    act
                      .Should().Throw<Exception>()
                      .Which.Message.Contains("проверьте корректность создания времени hours:-1,minutes:0,seconds:0,milliseconds:0");
                }

                [Fact]
                public void StoreAsVariableTimeLongWithFormat_CorrectVariable_ReturnTrue()
                {
                    // Act
                    var steps = new GeneratorSteps(variableController, featureContext);
                    var dt = new DateTime(1, 1, 1, 1, 1, 1, 0).ToString("T");

                    // Arrange
                    steps.StoreAsVariableTimeLongWithFormat(1, 1, 1, 0, "T", "test");

                    // Assert
                    var dateTime = variableController.GetVariableValue("test");
                    dateTime.Should().Be(dt);
                }

                [Fact]
                public void StoreAsVariableTimeLongWithFormat_IncorrectVariableName_ReturnException()
                {
                    // Act
                    var variable = new Variable() { Type = typeof(string), Value = string.Empty };
                    variableController.Variables.TryAdd("test", variable);

                    var steps = new GeneratorSteps(variableController, featureContext);

                    // Arrange
                    Action act = () => steps.StoreAsVariableTimeLongWithFormat(1, 1, 1, 0, "T", "test");

                    // Assert
                    act
                      .Should().Throw<Exception>()
                      .Which.Message.Contains("переменная \"test\" уже существует");
                }

                [Fact]
                public void StoreAsVariableTimeLongWithFormat_IncorrectDate_ReturnException()
                {
                    // Act
                    var steps = new GeneratorSteps(variableController, featureContext);

                    // Arrange
                    Action act = () => steps.StoreAsVariableTimeLongWithFormat(-1, 0, 0, 0, "T", "test");

                    // Assert
                    act
                      .Should().Throw<Exception>()
                      .Which.Message.Contains("проверьте корректность создания времени hours:-1,minutes:0,seconds:0,milliseconds:0");
                }
        #endregion
        #region StoreAsVariableDateTime
        [Fact]
        public void StoreAsVariableDateTime_CorrectVariable_ReturnTrue()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);
            var dt = new DateTime(2000, 1, 1, 1, 1, 1, 0);

            // Arrange
            steps.StoreAsVariableDateTime(1, 1, 2000, 1, 1, 1, "test");

            // Assert
            var dateTime = variableController.GetVariableValue("test");
            dateTime.Should().Be(dt);
        }

        [Fact]
        public void StoreAsVariableDateTime_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableDateTime(1, 1, 2000, 1, 1, 1, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }

        [Fact]
        public void StoreAsVariableDateTime_IncorrectDate_ReturnException()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange

            Action act = () => steps.StoreAsVariableDateTime(1, 1, 0, -1, 0, 0, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("проверьте корректность создания даты и времени day:1,month:1,year:0,hours:-1,minutes:0,seconds:0");
        }

        [Fact]
        public void StoreAsVariableDateTimeWithFormat_CorrectVariable_ReturnTrue()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);
            var dt = new DateTime(2000, 1, 1, 1, 1, 1, 0).ToString("G");

            // Arrange
            steps.StoreAsVariableDateTimeWithFormat(1, 1, 2000, 1, 1, 1, "G", "test");

            // Assert
            var dateTime = variableController.GetVariableValue("test");
            dateTime.Should().Be(dt);
        }

        [Fact]
        public void StoreAsVariableDateTimeWithFormat_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableDateTimeWithFormat(1, 1, 2000, 1, 1, 1, "G", "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }

        [Fact]
        public void StoreAsVariableDateTimeWithFormat_IncorrectDate_ReturnException()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableDateTimeWithFormat(1, 1, 0, -1, 0, 0, "G", "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("проверьте корректность создания даты и времени day:1,month:1,year:0,hours:-1,minutes:0,seconds:0");
        }
#endregion
        #region StoreAsVariableCurrentDate
        [Fact]
        public void StoreAsVariableCurrentDate_CorrectVariable_ReturnTrue()
        {
            // Act
            var mockDateTimeHelper = new Mock<IDateTimeHelper>();
            var fakeDate = new DateTime(2000, 1, 1, 1, 1, 1);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
                .Returns(fakeDate);

            var steps = new GeneratorSteps(variableController, featureContext);
            ((FakerGenerator)steps.fakerGenerator).DateTimeHelper.Value = mockDateTimeHelper.Object;

            // Arrange
            steps.StoreAsVariableCurrentDate("test");

            // Assert
            var dateTime = variableController.GetVariableValue("test");
            dateTime.Should().Be(fakeDate);
        }

        [Fact]
        public void StoreAsVariableCurrentDate_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableCurrentDate("test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }

        [Fact]
        public void StoreAsVariableCurrentDateWithFormat_CorrectVariable_ReturnTrue()
        {
            // Act
            var mockDateTimeHelper = new Mock<IDateTimeHelper>();
            var fakeDate = new DateTime(2000, 1, 1, 1, 1, 1);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
                .Returns(fakeDate);

            var steps = new GeneratorSteps(variableController, featureContext);
            ((FakerGenerator)steps.fakerGenerator).DateTimeHelper.Value = mockDateTimeHelper.Object;

            // Arrange
            steps.StoreAsVariableCurrentDateWithFormat("G","test");

            // Assert
            var dateTime = variableController.GetVariableValue("test");
            dateTime.Should().Be(fakeDate.ToString("G"));
        }

        [Fact]
        public void StoreAsVariableCurrentDateWithFormat_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableCurrentDateWithFormat("G", "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }
        #endregion
        #region StoreAsVariableRandomDateTime
        [Fact]
        public void StoreAsVariableRandomDateTime_CorrectVariable_ReturnTrue()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            steps.StoreAsVariableRandomDateTime("test");

            // Assert
            var dateTime = variableController.GetVariableValue("test");
            ((DateTime)dateTime).Should().BeAfter(Constants.START_DATETIME).And.BeBefore(Constants.LAST_DATETIME);
        }

        [Fact]
        public void StoreAsVariableRandomDateTime_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableRandomDateTime("test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }

        [Fact]
        public void StoreAsVariableRandomDateTimeWithFormat_CorrectVariable_ReturnTrue()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            steps.StoreAsVariableRandomDateTime("G", "test");

            // Assert
            var dateTime = variableController.GetVariableValue("test");

            DateTime.Parse((string)dateTime).Should().BeAfter(Constants.START_DATETIME).And.BeBefore(Constants.LAST_DATETIME);
        }

        [Fact]
        public void StoreAsVariableRandomDateTimeWithFormat_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableRandomDateTime("G", "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }
        #endregion
        #region StoreAsVariablePastDateTimeWithDifference
        [Fact]
        public void StoreAsVariablePastDateTimeWithDifference_CorrectVariable_ReturnTrue()
        {
            // Act
            var mockDateTimeHelper = new Mock<IDateTimeHelper>();
            var fakeDate = new DateTime(2000, 1, 1);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
                .Returns(fakeDate);

            var steps = new GeneratorSteps(variableController, featureContext);
            ((FakerGenerator)steps.fakerGenerator).DateTimeHelper.Value = mockDateTimeHelper.Object;

            var dt = new DateTime(1999, 1, 1);

            // Arrange
            steps.StoreAsVariablePastDateTimeWithDifference(1, 0, 0, "test");

            // Assert
            var dateTime = variableController.GetVariableValue("test");
            dateTime.Should().Be(dt);
        }

        [Fact]
        public void StoreAsVariablePastDateTimeWithDifference_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariablePastDateTimeWithDifference(1, 0, 0, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }

        [Fact]
        public void StoreAsVariablePastDateTimeWithDifference_IncorrectDate_ReturnException()
        {
            // Act
            var mockDateTimeHelper = new Mock<IDateTimeHelper>();
            var fakeDate = new DateTime(2000, 1, 1);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
                .Returns(fakeDate);

            var steps = new GeneratorSteps(variableController, featureContext);
            ((FakerGenerator)steps.fakerGenerator).DateTimeHelper.Value = mockDateTimeHelper.Object;

            // Arrange
            Action act = () => steps.StoreAsVariablePastDateTimeWithDifference(2001, 0, 0, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("проверьте корректность создания даты day:0,month:0,year:2001");
        }

        [Fact]
        public void StoreAsVariablePastDateTimeWithDifferenceWithFormat_CorrectVariable_ReturnTrue()
        {
            // Act
            var mockDateTimeHelper = new Mock<IDateTimeHelper>();
            var fakeDate = new DateTime(2000, 1, 1, 1, 1, 1);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
                .Returns(fakeDate);

            var steps = new GeneratorSteps(variableController, featureContext);
            ((FakerGenerator)steps.fakerGenerator).DateTimeHelper.Value = mockDateTimeHelper.Object;

            var dt = new DateTime(1999, 1, 1).ToString("yyyy-MM-dd");

            // Arrange
            steps.StoreAsVariablePastDateTimeWithDifference(1, 0, 0, "yyyy-MM-dd", "test");

            // Assert
            var dateTime = variableController.GetVariableValue("test");
            dateTime.Should().Be(dt);
        }

        [Fact]
        public void StoreAsVariablePastDateTimeWithDifferenceWithFormat_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariablePastDateTimeWithDifference(1, 0, 0, "yyyy-MM-dd", "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }

        [Fact]
        public void StoreAsVariablePastDateTimeWithDifferenceWithFormat_IncorrectDate_ReturnException()
        {
            // Act
            var mockDateTimeHelper = new Mock<IDateTimeHelper>();
            var fakeDate = new DateTime(2000, 1, 1);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
                .Returns(fakeDate);

            var steps = new GeneratorSteps(variableController, featureContext);
            ((FakerGenerator)steps.fakerGenerator).DateTimeHelper.Value = mockDateTimeHelper.Object;

            // Arrange
            Action act = () => steps.StoreAsVariablePastDateTimeWithDifference(2001, 0, 0, "yyyy-MM-dd", "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("проверьте корректность создания даты day:0,month:0,year:2001");
        }

        [Fact]
        public void StoreAsVariablePastDateTime_CorrectVariable_ReturnTrue()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);
            var dt = new DateTime(1999, 1, 1);

            // Arrange
            steps.StoreAsVariablePastDateTime(2000, 1, 1, 1, 0, 0, "test");

            // Assert
            var dateTime = variableController.GetVariableValue("test");
            dateTime.Should().Be(dt);
        }

        [Fact]
        public void StoreAsVariablePastDateTime_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariablePastDateTime(2000, 1, 1, 1, 0, 0, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }

        [Fact]
        public void StoreAsVariablePastDateTime_IncorrectDate_ReturnException()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariablePastDateTime(2000, 1, 1, 2001, 0, 0, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("проверьте корректность создания даты day:0,month:0,year:2001");
        }

        [Fact]
        public void StoreAsVariablePastDateTimeWithFormat_CorrectVariable_ReturnTrue()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            var dt = new DateTime(1999, 1, 1).ToString("yyyy-MM-dd");

            // Arrange
            steps.StoreAsVariablePastDateTime(2000, 1, 1, 1, 0, 0, "yyyy-MM-dd", "test");

            // Assert
            var dateTime = variableController.GetVariableValue("test");
            dateTime.Should().Be(dt);
        }

        [Fact]
        public void StoreAsVariablePastDateTimeWithFormat_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariablePastDateTime(2000, 1, 1, 1, 0, 0, "yyyy-MM-dd", "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }

        [Fact]
        public void StoreAsVariablePastDateTimeWithFormat_IncorrectDate_ReturnException()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariablePastDateTime(2000, 1, 1, 2001, 0, 0, "yyyy-MM-dd", "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("проверьте корректность создания даты day:0,month:0,year:2001");
        }
        #endregion
        #region StoreAsVariableFutureDateTimeWithDifference
        [Fact]
        public void StoreAsVariableFutureDateTimeWithDifference_CorrectVariable_ReturnTrue()
        {
            // Act
            var mockDateTimeHelper = new Mock<IDateTimeHelper>();
            var fakeDate = new DateTime(2000, 1, 1);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
                .Returns(fakeDate);

            var steps = new GeneratorSteps(variableController, featureContext);
            ((FakerGenerator)steps.fakerGenerator).DateTimeHelper.Value = mockDateTimeHelper.Object;

            var dt = new DateTime(2001, 1, 1);

            // Arrange
            steps.StoreAsVariableFutureDateTimeWithDifference(1, 0, 0, "test");

            // Assert
            var dateTime = variableController.GetVariableValue("test");
            dateTime.Should().Be(dt);
        }

        [Fact]
        public void StoreAsVariableFutureDateTimeWithDifference_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableFutureDateTimeWithDifference(1, 0, 0, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }

        [Fact]
        public void StoreAsVariableFutureDateTimeWithDifference_IncorrectDate_ReturnException()
        {
            // Act
            var mockDateTimeHelper = new Mock<IDateTimeHelper>();
            var fakeDate = new DateTime(2000, 1, 1);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
                .Returns(fakeDate);

            var steps = new GeneratorSteps(variableController, featureContext);
            ((FakerGenerator)steps.fakerGenerator).DateTimeHelper.Value = mockDateTimeHelper.Object;

            // Arrange
            Action act = () => steps.StoreAsVariablePastDateTimeWithDifference(10001, 0, 0, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("проверьте корректность создания даты day:0,month:0,year:10001");
        }

        [Fact]
        public void StoreAsVariableFutureDateTimeWithDifferenceWithFormat_CorrectVariable_ReturnTrue()
        {
            // Act
            var mockDateTimeHelper = new Mock<IDateTimeHelper>();
            var fakeDate = new DateTime(2000, 1, 1, 1, 1, 1);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
                .Returns(fakeDate);

            var steps = new GeneratorSteps(variableController, featureContext);
            ((FakerGenerator)steps.fakerGenerator).DateTimeHelper.Value = mockDateTimeHelper.Object;

            var dt = new DateTime(2001, 1, 1).ToString("yyyy-MM-dd");

            // Arrange
            steps.StoreAsVariableFutureDateTimeWithDifference(1, 0, 0, "yyyy-MM-dd", "test");

            // Assert
            var dateTime = variableController.GetVariableValue("test");
            dateTime.Should().Be(dt);
        }

        [Fact]
        public void StoreAsVariableFutureDateTimeWithDifferenceWithFormat_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableFutureDateTimeWithDifference(1, 0, 0, "yyyy-MM-dd", "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }

        [Fact]
        public void StoreAsVariableFutureDateTimeWithDifferenceWithFormat_IncorrectDate_ReturnException()
        {
            // Act
            var mockDateTimeHelper = new Mock<IDateTimeHelper>();
            var fakeDate = new DateTime(2000, 1, 1);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
                .Returns(fakeDate);

            var steps = new GeneratorSteps(variableController, featureContext);
            ((FakerGenerator)steps.fakerGenerator).DateTimeHelper.Value = mockDateTimeHelper.Object;

            // Arrange
            Action act = () => steps.StoreAsVariableFutureDateTimeWithDifference(10001, 0, 0, "yyyy-MM-dd", "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("проверьте корректность создания даты day:0,month:0,year:10001");
        }

        [Fact]
        public void StoreAsVariableFutureDateTime_CorrectVariable_ReturnTrue()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);
            var dt = new DateTime(2001, 1, 1);

            // Arrange
            steps.StoreAsVariableFutureDateTime(2000, 1, 1, 1, 0, 0, "test");

            // Assert
            var dateTime = variableController.GetVariableValue("test");
            dateTime.Should().Be(dt);
        }

        [Fact]
        public void StoreAsVariableFutureDateTime_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableFutureDateTime(2000, 1, 1, 1, 0, 0, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }

        [Fact]
        public void StoreAsVariableFutureDateTime_IncorrectDate_ReturnException()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableFutureDateTime(2000, 1, 1, 10001, 0, 0, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("проверьте корректность создания даты day:0,month:0,year:10001");
        }

        [Fact]
        public void StoreAsVariableFutureDateTimeWithFormat_CorrectVariable_ReturnTrue()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            var dt = new DateTime(2001, 1, 1).ToString("yyyy-MM-dd");

            // Arrange
            steps.StoreAsVariableFutureDateTime(2000, 1, 1, 1, 0, 0, "yyyy-MM-dd", "test");

            // Assert
            var dateTime = variableController.GetVariableValue("test");
            dateTime.Should().Be(dt);
        }

        [Fact]
        public void StoreAsVariableFutureDateTimeWithFormat_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableFutureDateTime(2000, 1, 1, 1, 0, 0, "yyyy-MM-dd", "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }

        [Fact]
        public void StoreAsVariableFutureDateTimeWithFormat_IncorrectDate_ReturnException()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableFutureDateTime(2000, 1, 1, 10001, 0, 0, "yyyy-MM-dd", "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("проверьте корректность создания даты day:0,month:0,year:10001");
        }
        #endregion
        #region StoreAsVariableRandomString/Char/Number with prefix/postfix
        [Fact]
        public void StoreAsVariableRandomStringWithPrefix_CorrectVariable_ReturnTrue()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);
            
            // Arrange
            steps.StoreAsVariableRandomStringWithPrefix(10, _prepostfix, "test");

            // Assert
            var str = variableController.GetVariableValue("test");
            ((string)str).Should().StartWith(_prepostfix);
        }

        [Fact]
        public void StoreAsVariableRandomStringWithPrefix_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableRandomStringWithPrefix(10, _prepostfix, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }

        [Fact]
        public void StoreAsVariableRandomStringWithPrefix_IncorrectDate_ReturnException()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange

            Action act = () => steps.StoreAsVariableRandomStringWithPrefix(1, _prepostfix, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("postfix and/or prefix are longer than the string itself");
        }

        [Fact]
        public void StoreAsVariableRandomCharWithPrefix_CorrectVariable_ReturnTrue()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            steps.StoreAsVariableRandomCharWithPrefix(10, _prepostfix, "test");

            // Assert
            var str = variableController.GetVariableValue("test");
            ((string)str).Should().StartWith(_prepostfix);
        }

        [Fact]
        public void StoreAsVariableRandomCharWithPrefix_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableRandomCharWithPrefix(10, _prepostfix, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }

        [Fact]
        public void StoreAsVariableRandomCharWithPrefix_IncorrectDate_ReturnException()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange

            Action act = () => steps.StoreAsVariableRandomCharWithPrefix(1, _prepostfix, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("postfix and/or prefix are longer than the string itself");
        }

        [Fact]
        public void StoreAsVariableRandomNumberWithPrefix_CorrectVariable_ReturnTrue()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            steps.StoreAsVariableRandomNumberWithPrefix(10, _prepostfix, "test");

            // Assert
            var str = variableController.GetVariableValue("test");
            ((string)str).Should().StartWith(_prepostfix);
        }

        [Fact]
        public void StoreAsVariableRandomNumberWithPrefix_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableRandomNumberWithPrefix(10, _prepostfix, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }

        [Fact]
        public void StoreAsVariableRandomNumberWithPrefix_IncorrectDate_ReturnException()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange

            Action act = () => steps.StoreAsVariableRandomNumberWithPrefix(1, _prepostfix, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("postfix and/or prefix are longer than the string itself");
        }

        [Fact]
        public void StoreAsVariableRandomStringWithPostFix_CorrectVariable_ReturnTrue()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            steps.StoreAsVariableRandomStringWithPostFix(10, _prepostfix, "test");

            // Assert
            var str = variableController.GetVariableValue("test");
            ((string)str).Should().EndWith(_prepostfix);
        }

        [Fact]
        public void StoreAsVariableRandomStringWithPostFix_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableRandomStringWithPostFix(10, _prepostfix, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }

        [Fact]
        public void StoreAsVariableRandomStringWithPostFix_IncorrectDate_ReturnException()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange

            Action act = () => steps.StoreAsVariableRandomStringWithPostFix(1, _prepostfix, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("postfix and/or prefix are longer than the string itself");
        }

        [Fact]
        public void StoreAsVariableRandomCharWithPostfix_CorrectVariable_ReturnTrue()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            steps.StoreAsVariableRandomCharWithPostfix(10, _prepostfix, "test");

            // Assert
            var str = variableController.GetVariableValue("test");
            ((string)str).Should().EndWith(_prepostfix);
        }

        [Fact]
        public void StoreAsVariableRandomCharWithPostfix_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableRandomCharWithPostfix(10, _prepostfix, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }

        [Fact]
        public void StoreAsVariableRandomCharWithPostfix_IncorrectDate_ReturnException()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange

            Action act = () => steps.StoreAsVariableRandomCharWithPostfix(1, _prepostfix, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("postfix and/or prefix are longer than the string itself");
        }

        [Fact]
        public void StoreAsVariableRandomNumberWithPostfix_CorrectVariable_ReturnTrue()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            steps.StoreAsVariableRandomNumberWithPostfix(10, _prepostfix, "test");

            // Assert
            var str = variableController.GetVariableValue("test");
            ((string)str).Should().EndWith(_prepostfix);
        }

        [Fact]
        public void StoreAsVariableRandomNumberWithPostfix_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableRandomNumberWithPostfix(10, _prepostfix, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }

        [Fact]
        public void StoreAsVariableRandomNumberWithPostfix_IncorrectDate_ReturnException()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange

            Action act = () => steps.StoreAsVariableRandomNumberWithPostfix(1, _prepostfix, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("postfix and/or prefix are longer than the string itself");
        }
        #endregion
        #region StoreAsVariableRandomString/Char/Number
        [Fact]
        public void StoreAsVariableRandomString_CorrectVariable_ReturnTrue()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            steps.StoreAsVariableRandomString(10, "test");

            // Assert
            var str = variableController.GetVariableValue("test");
            ((string)str).Should().HaveLength(10);
        }

        [Fact]
        public void StoreAsVariableRandomString_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableRandomString(10, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }

        [Fact]
        public void StoreAsVariableRandomString_IncorrectDate_ReturnException()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange

            Action act = () => steps.StoreAsVariableRandomString(0, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("the length must be positive, but found 0.");
        }

        [Fact]
        public void StoreAsVariableRandomChar_CorrectVariable_ReturnTrue()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            steps.StoreAsVariableRandomChar(10, "test");

            // Assert
            var str = variableController.GetVariableValue("test");
            ((string)str).Should().HaveLength(10);
        }

        [Fact]
        public void StoreAsVariableRandomChar_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableRandomChar(10, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }

        [Fact]
        public void StoreAsVariableRandomChar_IncorrectDate_ReturnException()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange

            Action act = () => steps.StoreAsVariableRandomChar(0, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("the length must be positive, but found 0.");
        }

        [Fact]
        public void StoreAsVariableRandomNumber_CorrectVariable_ReturnTrue()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            steps.StoreAsVariableRandomNumber(10, "test");

            // Assert
            var str = variableController.GetVariableValue("test");
            ((string)str).Should().HaveLength(10);
        }

        [Fact]
        public void StoreAsVariableRandomNumber_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableRandomNumber(10, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }

        [Fact]
        public void StoreAsVariableRandomNumber_IncorrectDate_ReturnException()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange

            Action act = () => steps.StoreAsVariableRandomNumber(0, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("the length must be positive, but found 0.");
        }
        #endregion
        #region StoreAsVariableRandomPhone
        [Theory]
        [InlineData("+7##########"), InlineData("+7###-###-##-##"), InlineData("+7 ### ### ## ##"), InlineData("+7(###)###-##-##"), InlineData("+7###########"), InlineData("+7 ### ### ## ## ## ##"), InlineData("+7#"), InlineData("+7")]
        public void StoreAsVariableRandomPhone_CorrectVariable_ReturnTrue(string mask)
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            steps.StoreAsVariableRandomPhone(mask, "test");

            // Assert
            var str = variableController.GetVariableValue("test");
            ((string)str).Should().HaveLength(mask.Length);
        }

        [Fact]
        public void StoreAsVariableRandomPhone_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableRandomString(10, "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }
        #endregion
        #region StoreAsVariableUuid
        [Fact]
        public void StoreAsVariableUuid_CorrectVariable_ReturnTrue()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            steps.StoreAsVariableUuid("test");

            // Assert
            var guid = (string)variableController.GetVariableValue("test");
            var isValid = Guid.TryParse(guid, out _);
            isValid.Should().BeTrue();
        }

        [Fact]
        public void StoreAsVariableUuid_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableUuid("test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }
        #endregion
        #region StoreAsVariableMonth
        [Fact]
        public void StoreAsVariableMonth_CorrectVariable_ReturnTrue()
        {
            // Act
            var months = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            steps.StoreAsVariableMonth("test");

            // Assert
            string month = (string)variableController.GetVariableValue("test");
            months.Should().Contain(month);
        }

        [Fact]
        public void StoreAsVariableMonth_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableMonth("test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }
        #endregion
        #region StoreAsVariableWeekday
        [Fact]
        public void StoreAsVariableWeekday_CorrectVariable_ReturnTrue()
        {
            // Act
            var weekdays = new List<string> { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            steps.StoreAsVariableWeekday("test");

            // Assert
            string weekday = (string)variableController.GetVariableValue("test");
            weekdays.Should().Contain(weekday);
        }

        [Fact]
        public void StoreAsVariableWeekday_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableWeekday("test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }
        #endregion
        #region StoreAsVariableEmail
        [Fact]
        public void StoreAsVariableEmail_CorrectVariable_ReturnTrue()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            steps.StoreAsVariableEmail("mail","test");

            // Assert
            string email = (string)variableController.GetVariableValue("test");
            string address = new System.Net.Mail.MailAddress(email).Address;
            address.Should().Be(email);
            address.Should().EndWith("mail");
        }

        [Fact]
        public void StoreAsVariableEmail_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableEmail("mail", "test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }
        #endregion
        #region StoreAsVariableIp
        [Fact]
        public void StoreAsVariableIp_CorrectVariable_ReturnTrue()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            steps.StoreAsVariableIp("test");

            // Assert
            var ip = (string)variableController.GetVariableValue("test");
            var isValid = IPAddress.TryParse(ip, out _);
            isValid.Should().BeTrue();
        }

        [Fact]
        public void StoreAsVariableIp_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableIp("test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }
        #endregion
        #region StoreAsVariableUrl
        [Fact]
        public void StoreAsVariableUrl_CorrectVariable_ReturnTrue()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            steps.StoreAsVariableUrl("test");

            // Assert
            var url = (string)variableController.GetVariableValue("test");
            var isValid = Uri.TryCreate(url, UriKind.Absolute, out _);
            isValid.Should().BeTrue();
        }

        [Fact]
        public void StoreAsVariableUrl_IncorrectVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreAsVariableUrl("test");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" уже существует");
        }
        #endregion
        #region StoreVariableValueToArrayVariable
        [Theory]
        [InlineData("a,b,c", ",", 3), InlineData("a,b!c.d", ",.!", 4)]
        public void StoreVariableValueToArrayVariable_CorrectVariable_ReturnTrue(string str, string chars, int count)
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = str };
            variableController.Variables.TryAdd("test", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            steps.StoreVariableValueToArrayVariable("test", chars, "nTest");

            // Assert
            var arr = variableController.GetVariableValue("nTest");
            var array = (IEnumerable<object>)variableController.GetVariableValue("nTest");
            array.Should().HaveCount(count);
        }

        [Fact]
        public void StoreVariableValueToArrayVariable_IncorrectVariableName_ReturnException()
        {
            // Act
            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreVariableValueToArrayVariable("test", "a", "nTest");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"test\" не существует");
        }

        [Fact]
        public void StoreVariableValueToArrayVariable_IncorrectNewVariableName_ReturnException()
        {
            // Act
            var variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("test", variable);
            variable = new Variable() { Type = typeof(string), Value = string.Empty };
            variableController.Variables.TryAdd("nTest", variable);

            var steps = new GeneratorSteps(variableController, featureContext);

            // Arrange
            Action act = () => steps.StoreVariableValueToArrayVariable("test", "a", "nTest");

            // Assert
            act
              .Should().Throw<Exception>()
              .Which.Message.Contains("переменная \"nTest\" уже существует");
        }
        #endregion
    }
}
