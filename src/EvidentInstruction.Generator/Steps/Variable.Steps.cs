using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Xml.Linq;
using FluentAssertions;
using EvidentInstruction.Controllers;
using EvidentInstruction.Helpers;
using TechTalk.SpecFlow;
using Microsoft.Extensions.Logging;

namespace EvidentInstruction.Generator.Steps
{
    /// <summary>
    /// Общие шаги для работы с переменными.
    /// </summary>
    [Binding]
    public class VariableSteps
    {
        private readonly VariableController variableController;

        /// <summary>
        /// Initializes a new instance of the <see cref="Generator"/> class.
        /// Привязка общих шагов к работе с переменным через контекст.
        /// </summary>
        /// <param name="variableController">Контекст для работы с переменными.</param>
        public VariableSteps(VariableController variableController)
        {
            this.variableController = variableController;
        }

        /// <summary>
        /// Шаг для явного ожидания.
        /// </summary>
        /// <param name="seconds">Количество секунд ожидания.</param>
        [ExcludeFromCodeCoverage]
        [StepDefinition(@"я жду ([0-9]+) сек\.")]
        public void WaitForSeconds(int seconds)
        {
            seconds.Should().BePositive("Waiting time must be greater");
            seconds.Should().NotBe(0, "Waiting time cannot be equals zero");
            Thread.Sleep(seconds * 1000);
        }

        /// <summary>
        /// Шаг для удаления переменной.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я удаляю переменную ""(.+)""")]
        public void DeleteVariable(string varName)
        {
            this.variableController.Variables.ContainsKey(varName).Should().BeTrue($"Переменной \"{varName}\" не существует");
            this.variableController.Variables.TryRemove(varName, out var variable);
        }

        /// <summary>
        /// Шаг для очистки значения переменной.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я очищаю переменную ""(.+)""")]
        public void EmtpyVariable(string varName)
        {
            this.variableController.Variables.ContainsKey(varName).Should().BeTrue($"Переменной \"{varName}\" не существует");
            this.variableController.SetVariable(varName, typeof(object), null);
        }

        /// <summary>
        /// Шаг для изменения значения переменной.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="value">Значение переменной.</param>
        [StepDefinition(@"я изменяю значение переменной ""(.+)"" на ""(.+)""")]
        public void ChangeVariable(string varName, object value)
        {
            this.variableController.Variables.ContainsKey(varName).Should().BeTrue($"Переменной \"{varName}\" не существует");
            this.variableController.SetVariable(varName, value.GetType(), value);
        }

        /// <summary>
        /// Шаг для сохранения значения однострочного текста в переменную.
        /// </summary>
        /// <param name="text">Текст для сохранения в переменную.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю текст ""(.*)"" в переменную ""(.+)""")]
        public void StoreAsVariableString(string text, string varName)
        {
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная \"{varName}\" уже существует");
            this.variableController.SetVariable(varName, typeof(string), text);
        }

        /// <summary>
        /// Шаг для сохранения зашифрованного значения однострочного текста в переменную.
        /// </summary>
        /// <param name="text">Зашифрованный текст для сохранения в переменную.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю зашифрованный текст ""(.*)"" в переменную ""(.+)""")]
        public void StoreAsVariableEncriptedString(string text, string varName)
        {
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная \"{varName}\" уже существует");
            this.variableController.SetVariable(varName, typeof(string), Encryptor.Decrypt(text));
        }

        /// <summary>
        /// Шаг для сохранения многострочного текста в переменную.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="text">Текст для сохранения в переменную.</param>
        [StepDefinition(@"я сохраняю текст в переменную ""(.+)"":")]
        public void StoreAsVariableText(string varName, string text)
        {
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная \"{varName}\" уже существует");
            this.variableController.SetVariable(varName, typeof(string), text);
        }

        /// <summary>
        /// Шаг для сохранения числа в переменную (float, int).
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="number">Число.</param>
        [StepDefinition(@"я сохраняю число ""(.+)"" в переменную ""(.*)""")]
        public void StoreAsVariableNumber(string varName, string number)
        {
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная \"{varName}\" уже существует");
            if (decimal.TryParse(number, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.CurrentCulture, out var dec))
            {
                this.variableController.SetVariable(varName, typeof(decimal), dec);
                return;
            }

            if (decimal.TryParse(number, System.Globalization.NumberStyles.Float, new System.Globalization.NumberFormatInfo() { PercentDecimalSeparator = ".", CurrencyDecimalSeparator = ".", NumberDecimalSeparator = "." }, out dec))
            {
                this.variableController.SetVariable(varName, typeof(decimal), dec);
                return;
            }

            if (decimal.TryParse(number, System.Globalization.NumberStyles.Float, new System.Globalization.NumberFormatInfo() { PercentDecimalSeparator = ",", CurrencyDecimalSeparator = ",", NumberDecimalSeparator = "," }, out dec))
            {
                this.variableController.SetVariable(varName, typeof(decimal), dec);
                return;
            }

            this.variableController.SetVariable(varName, typeof(int), int.Parse(number));
        }

        /// <summary>
        /// Шаг для сохранения XML документа в переменную.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="xml">Текст, представленный в виде XML.</param>
        [StepDefinition(@"я сохраняю текст как XML документ в переменную ""(.+)"":")]
        public void StoreAsVariableXmlFromText(string varName, string xml)
        {
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная \"{varName}\" уже существует");
            var xmlBody = this.variableController.ReplaceVariables(xml);

            Log.Logger().LogInformation($"xml is \"{xmlBody}\"");

            var doc = Converter.CreateXmlDoc(xmlBody);
            doc.Should().NotBeNull($"Создать XmlDoc из строки \"{xmlBody}\" не удалось");

            this.variableController.SetVariable(varName, doc.GetType(), doc);
        }

        /// <summary>
        /// Шаг для сохранения значения одной переменной в другую.
        /// </summary>
        /// <param name="varName">Исходная переменная.</param>
        /// <param name="newVarName">Переменная-результат.</param>
        [StepDefinition(@"я сохраняю значение переменной ""(.+)"" в переменную ""(.+)""")]
        public void StoreVariableValueToVariable(string varName, string newVarName)
        {
            this.variableController.Variables.ContainsKey(this.variableController.GetVariableName(varName)).Should().BeTrue($"Переменной \"{varName}\" не существует");
            this.variableController.Variables.ContainsKey(newVarName).Should().BeFalse($"Переменная \"{newVarName}\" уже существует");
            var variable = this.variableController.GetVariable(varName);
            variable.Should().NotBeNull($"Значения в переменной \"{varName}\" нет");

            this.variableController.SetVariable(newVarName, variable.Type, variable.Value);
        }


        /// <summary>
        /// Шаг сохранения результата значения переменной, содержащей cdata в переменную.
        /// </summary>
        /// <param name="cdata">Переменная с cdata.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю значение переменной \""(.+)\"" из CDATA в переменную \""(.+)\""")]
        public void StoreCDataVariable_ToVariable(string cdataVar, string varName)
        {
            this.variableController.Variables.ContainsKey(this.variableController.GetVariableName(cdataVar)).Should().BeTrue($"Переменной \"{cdataVar}\" не существует");
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная \"{varName}\" уже существует");

            var value = (string)this.variableController.GetVariableValue(varName);
            var cdata = Converter.CreateCData(value);
            cdata.Should().NotBeNull($"Значение переменной \"{Environment.NewLine + cdata + Environment.NewLine}\" не является CDATA");

            this.variableController.SetVariable(varName, typeof(XDocument), cdata);
        }

        /// <summary>
        /// Шаг для подстановки значения переменной в текст и сохранения результата в новую переменную.
        /// </summary>
        /// <param name="varName">Идентификатор исходной переменной.</param>
        /// <param name="text">Текст.</param>
        /// <param name="newVarName">Идентификатор результирующей переменной.</param>
        [StepDefinition(@"я подставляю значение переменной ""(.+)"" в текст ""(.*)"" и сохраняю в переменную ""(.+)""")]
        public void StoreAsVariableStringFormat(string varName, string text, string newVarName)
        {
            this.variableController.Variables.ContainsKey(varName).Should().BeTrue($"Переменной \"{varName}\" не существует");
            this.variableController.Variables.ContainsKey(newVarName).Should().BeFalse($"Переменная \"{newVarName}\" уже существует");
            this.variableController.GetVariableValue(varName).Should().NotBeNull($"Значения в переменной \"{varName}\" нет");

            var replacement = string.Empty;

            if (this.variableController.Variables.ContainsKey(varName) &&
                this.variableController.GetVariableValue(varName) != null)
            {
                if (this.variableController.Variables[varName].Type == typeof(string))
                {
                    replacement = (string)this.variableController.GetVariableValue(varName);
                }
                else
                {
                    replacement = this.variableController.GetVariableValue(varName).ToString();
                }
            }

            this.variableController.SetVariable(newVarName, typeof(string), text?.Replace($"{{{varName}}}", replacement));
        }

        /// <summary>
        /// Шаг проверки, что значение переменной не является NULL.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" не является NULL")]
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" существует")]
        public void CheckVariableIsNotNull(string varName)
        {
            string.IsNullOrWhiteSpace(varName).Should().BeFalse($"Значение \"varName\" не задано.");
            var value = this.variableController.GetVariableValue(varName);
            value.Should().NotBeNull($"Значение переменной \"{varName}\" является NULL");
        }

        /// <summary>
        /// Шаг проверки, что значение переменной является NULL.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" является NULL")]
        [Then(@"я убеждаюсь, что значения переменной ""(.+)"" нет")]
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" не существует")]
        public void CheckVariableIsNull(string varName)
        {
            string.IsNullOrWhiteSpace(varName).Should().BeFalse($"Значение \"varName\" не задано.");
            var value = this.variableController.GetVariableValue(varName);
            value.Should().BeNull($"Значение переменной \"{varName}\" не является NULL");
        }

        /// <summary>
        /// Шаг проверки, что значение переменной не является пустой строкой.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" не является пустой строкой")]
        public void CheckVariableIsNotEmpty(string varName)
        {
            string.IsNullOrWhiteSpace(varName).Should().BeFalse($"Значение \"varName\" не задано.");
            var value = this.variableController.GetVariableValue(varName);
            value.Should().NotBeNull($"Значения в переменной \"{varName}\" нет");
            if (this.variableController.GetVariable(varName)?.Type == typeof(string))
            {
                string.IsNullOrWhiteSpace((string)value).Should().BeFalse($"Значение переменной \"{varName}\" пустая строка");
            }
        }

        /// <summary>
        /// Шаг проверки, что значение переменной  является пустой строкой.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" пустая строка")]
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" равно пустой строке")]
        public void CheckVariableIsEmpty(string varName)
        {
            string.IsNullOrWhiteSpace(varName).Should().BeFalse($"Значение \"varName\" не задано.");
            var value = this.variableController.GetVariableValue(varName);
            value.Should().NotBeNull($"Значения в переменной \"{varName}\" нет");
            if (this.variableController.GetVariable(varName)?.Type == typeof(string))
            {
                string.IsNullOrWhiteSpace((string)value).Should().BeTrue($"Значение переменной \"{varName}\" не пустая строка");
            }
        }

        /// <summary>
        /// Шаг проверки, что значение переменной равно переданному объекту.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="expected">Expected значение.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" равно ""(.+)""")]
        public void CheckVariableEquals(string varName, object expected)
        {
            string.IsNullOrWhiteSpace(varName).Should().BeFalse($"Значение \"varName\" не задано.");
            expected.Should().NotBeNull($"Значение \"expected\" не задано.");

            var actual = this.variableController.GetVariableValueText(varName);
            expected.GetType().Should().Be(actual.GetType(), $"Тип значения переменной \"{varName}\" не совпадает с типом \"{expected}\"");
            expected.Should().Be(actual, $"Значение переменной \"{varName}\":\"{actual}\" не равно \"{expected}\"");
        }

        /// <summary>
        /// Шаг проверки, что значение переменной не равно переданному объекту.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="expected">Expected значение.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" не равно ""(.+)""")]
        public void CheckVariableNotEquals(string varName, object expected)
        {
            string.IsNullOrWhiteSpace(varName).Should().BeFalse($"Значение \"varName\" не задано.");
            expected.Should().NotBeNull($"Значение \"expected\" не задано.");

            var actual = this.variableController.GetVariableValueText(varName);
            expected.GetType().Should().Be(actual.GetType(), $"Тип значения переменной \"{varName}\" не совпадает с типом \"{expected}\"");
            expected.Should().NotBe(actual, $"Значение переменной \"{varName}\":\"{actual}\" равно \"{expected}\"");
        }

        /// <summary>
        /// Шаг проверки того, что значение переменной содержит строку.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="expected">Expected значение.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" содержит ""(.+)""")]
        public void CheckVariableContains(string varName, string expected)
        {
            string.IsNullOrWhiteSpace(varName).Should().BeFalse($"Значение \"varName\" не задано.");
            string.IsNullOrWhiteSpace(expected).Should().BeFalse($"Значение \"expected\" не задано.");

            var actual = this.variableController.GetVariableValueText(varName);
            actual.Should().NotBeNull($"Значение переменной \"{varName}\" NULL.");
            actual.Contains(expected).Should().BeTrue($"Значение переменной \"{varName}\":\"{actual}\" не содержит \"{expected}\"");
        }

        /// <summary>
        /// Шаг проверки того, что значение переменной не содержит строку.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="expected">Expected значение.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" не содержит ""(.+)""")]
        public void CheckVariableNotContains(string varName, string expected)
        {
            string.IsNullOrWhiteSpace(varName).Should().BeFalse($"Значение \"varName\" не задано.");
            string.IsNullOrWhiteSpace(expected).Should().BeFalse($"Значение \"expected\" не задано.");

            var actual = this.variableController.GetVariableValueText(varName);
            actual.Should().NotBeNull($"Значение переменной \"{varName}\" NULL.");
            actual.Contains(expected).Should().BeFalse($"Значение переменной \"{varName}\":\"{actual}\" содержит \"{expected}\"");
        }

        /// <summary>
        /// Шаг проверки того, что значение переменной начинается со строки.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="expected">Expected значение.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" начинается с ""(.+)""")]
        public void CheckVariableStartsWith(string varName, string expected)
        {
            string.IsNullOrWhiteSpace(varName).Should().BeFalse($"Значение \"varName\" не задано.");
            string.IsNullOrWhiteSpace(expected).Should().BeFalse($"Значение \"expected\" не задано.");
            var actual = this.variableController.GetVariableValueText(varName);
            actual.Should().NotBeNull($"Значение переменной \"{varName}\" NULL.");
            actual.StartsWith(expected).Should().BeTrue($"Значение переменной \"{varName}\":\"{actual}\" не начинается с \"{expected}\"");
        }

        /// <summary>
        /// Шаг проверки того, что значение переменной закачивается строкой.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="expected">Expected значение.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" заканчивается с ""(.+)""")]
        public void CheckVariableEndsWith(string varName, string expected)
        {
            string.IsNullOrWhiteSpace(varName).Should().BeFalse($"Значение \"varName\" не задано.");
            string.IsNullOrWhiteSpace(expected).Should().BeFalse($"Значение \"expected\" не задано.");
            var actual = this.variableController.GetVariableValueText(varName);
            actual.Should().NotBeNull($"Значение переменной \"{varName}\" NULL.");
            actual.EndsWith(expected).Should().BeTrue($"Значение переменной \"{varName}\":\"{actual}\" не заканчивается с \"{expected}\"");
        }

        /// <summary>
        /// Шаг проверки того, что значение одной переменной равно значению другой переменной.
        /// </summary>
        /// <param name="varName1">Переменная 1.</param>
        /// <param name="varName2">Переменная 2.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" равно значению переменной ""(.+)""")]
        public void CheckVariablesAreEqual(string varName1, string varName2)
        {
            string.IsNullOrWhiteSpace(varName1).Should().BeFalse($"Значение \"varName1\" не задано.");
            string.IsNullOrWhiteSpace(varName2).Should().BeFalse($"Значение \"varName2\" не задано.");

            var value1 = this.variableController.GetVariableValueText(varName1);
            var value2 = this.variableController.GetVariableValueText(varName2);

            value1.Should().NotBeNull($"Значения в переменной \"{varName1}\" нет");
            value2.Should().NotBeNull($"Значения в переменной \"{varName2}\" нет");

            value1.Should().Be(
                value2,
                $"Значение переменной \"{varName1}\":\"{value1}\" не равно значению переменной \"{varName2}\":\"{value2}\"");
        }

        /// <summary>
        /// Шаг проверки того, что значение одной переменной не равно значению другой переменной.
        /// </summary>
        /// <param name="varName1">Переменная 1.</param>
        /// <param name="varName2">Переменная 2.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" не равно значению переменной ""(.+)""")]
        public void CheckVariablesAreNotEqual(string varName1, string varName2)
        {
            string.IsNullOrWhiteSpace(varName1).Should().BeFalse($"Значение \"varName1\" не задано.");
            string.IsNullOrWhiteSpace(varName2).Should().BeFalse($"Значение \"varName2\" не задано.");

            var value1 = this.variableController.GetVariableValueText(varName1);
            var value2 = this.variableController.GetVariableValueText(varName2);

            value1.Should().NotBeNull($"Значения в переменной \"{varName1}\" нет");
            value2.Should().NotBeNull($"Значения в переменной \"{varName2}\" нет");

            value1.Should().NotBe(
                value2,
                $"Значение переменной \"{varName1}\":\"{value1}\" равно значению переменной \"{varName2}\":\"{value2}\"");
        }

        /// <summary>
        /// Шаг проверки того, что значение одной переменной содержит значение другой переменной.
        /// </summary>
        /// <param name="varName1">Переменная 1.</param>
        /// <param name="varName2">Переменная 2.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" содержит значение переменной ""(.+)""")]
        public void CheckVariableAreContains(string varName1, string varName2)
        {
            string.IsNullOrWhiteSpace(varName1).Should().BeFalse($"Значение \"varName1\" не задано.");
            string.IsNullOrWhiteSpace(varName2).Should().BeFalse($"Значение \"varName2\" не задано.");

            var value1 = this.variableController.GetVariableValueText(varName1);
            var value2 = this.variableController.GetVariableValueText(varName2);

            value1.Should().NotBeNull($"Значения в переменной \"{varName1}\" нет");
            value2.Should().NotBeNull($"Значения в переменной \"{varName2}\" нет");

            value1.Contains(value2).Should().BeTrue($"Значение переменной \"{varName1}\":\"{value1}\" не содержит значение переменной \"{varName2}\":\"{value2}\"");
        }

        /// <summary>
        /// Шаг проверки того, что значение одной переменной не содержит значение другой переменной.
        /// </summary>
        /// <param name="varName1">Переменная 1.</param>
        /// <param name="varName2">Переменная 2.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" не содержит значение переменной ""(.+)""")]
        public void CheckVariableAreNotContains(string varName1, string varName2)
        {
            string.IsNullOrWhiteSpace(varName1).Should().BeFalse($"Значение \"varName1\" не задано.");
            string.IsNullOrWhiteSpace(varName2).Should().BeFalse($"Значение \"varName2\" не задано.");

            var value1 = this.variableController.GetVariableValueText(varName1);
            var value2 = this.variableController.GetVariableValueText(varName2);

            value1.Should().NotBeNull($"Значения в переменной \"{varName1}\" нет");
            value2.Should().NotBeNull($"Значения в переменной \"{varName2}\" нет");

            value1.Contains(value2).Should().BeFalse($"Значение переменной \"{varName1}\":\"{value1}\" содержит значение переменной \"{varName2}\":\"{value2}\"");
        }
    }
}