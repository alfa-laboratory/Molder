using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Xml.Linq;
using FluentAssertions;
using Molder.Controllers;
using Molder.Helpers;
using TechTalk.SpecFlow;
using Microsoft.Extensions.Logging;

namespace Molder.Generator.Steps
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
            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            this.variableController.Variables.TryRemove(varName, out var variable);
        }

        /// <summary>
        /// Шаг для очистки значения переменной.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я очищаю переменную ""(.+)""")]
        public void EmtpyVariable(string varName)
        {
            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
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
            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
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
            var xmlBody = this.variableController.ReplaceVariables(xml);

            Log.Logger().LogInformation($"xml is \"{xmlBody}\"");

            var doc = Converter.CreateXmlDoc(xmlBody);
            doc.Should().NotBeNull($"создать XmlDoc из строки \"{xmlBody}\" не удалось");

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
            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            var value = this.variableController.GetVariableValue(varName);
            value.Should().NotBeNull($"значения в переменной \"{varName}\" нет");

            this.variableController.SetVariable(newVarName, value.GetType(), value);
        }

        /// <summary>
        /// Шаг сохранения результата значения переменной, содержащей cdata в переменную.
        /// </summary>
        /// <param name="cdata">Переменная с cdata.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю значение переменной \""(.+)\"" из CDATA в переменную \""(.+)\""")]
        public void StoreCDataVariable_ToVariable(string cdataVar, string varName)
        {
            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{cdataVar}\" не существует");

            var value = (string)this.variableController.GetVariableValue(varName);
            var cdata = Converter.CreateCData(value);
            cdata.Should().NotBeNull($"значение переменной \"{Environment.NewLine + cdata + Environment.NewLine}\" не является CDATA");

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
            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");

            var replacement = string.Empty;

            if (this.variableController.GetVariableValue(varName) != null)
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
            var value = this.variableController.GetVariableValue(varName);
            value.Should().NotBeNull($"значение переменной \"{varName}\" является NULL");
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
            var value = this.variableController.GetVariableValue(varName);
            value.Should().BeNull($"значение переменной \"{varName}\" не является NULL");
        }

        /// <summary>
        /// Шаг проверки, что значение переменной не является пустой строкой.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" не является пустой строкой")]
        public void CheckVariableIsNotEmpty(string varName)
        {
            var value = this.variableController.GetVariableValue(varName);
            value.Should().NotBeNull($"значения в переменной \"{varName}\" нет");
            if (this.variableController.GetVariable(varName)?.Type == typeof(string))
            {
                string.IsNullOrWhiteSpace((string)value).Should().BeFalse($"значение переменной \"{varName}\" пустая строка");
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
            var value = this.variableController.GetVariableValue(varName);
            value.Should().NotBeNull($"значения в переменной \"{varName}\" нет");
            if (this.variableController.GetVariable(varName)?.Type == typeof(string))
            {
                string.IsNullOrWhiteSpace((string)value).Should().BeTrue($"значение переменной \"{varName}\" не пустая строка");
            }
        }

        /// <summary>
        /// Шаг проверки, что значение переменной равно переданному объекту.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="expected">Expected значение.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" равно ""(.+)""")]
        public void CheckVariableEquals(string varName, string expected)
        {
            expected.Should().NotBeNull($"значение \"expected\" не задано");
            expected = this.variableController.ReplaceVariables(expected) ?? expected;

            var actual = this.variableController.GetVariableValueText(varName);
            actual.Should().NotBeNull($"значения в переменной \"{varName}\" нет");
            expected.Should().Be(actual, $"значение переменной \"{varName}\":\"{actual}\" не равно \"{expected}\"");
        }

        /// <summary>
        /// Шаг проверки, что значение переменной не равно переданному объекту.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="expected">Expected значение.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" не равно ""(.+)""")]
        public void CheckVariableNotEquals(string varName, string expected)
        {
            expected.Should().NotBeNull($"значение \"expected\" не задано");
            expected = this.variableController.ReplaceVariables(expected) ?? expected;

            var actual = this.variableController.GetVariableValueText(varName);
            actual.Should().NotBeNull($"значения в переменной \"{varName}\" нет");
            expected.Should().NotBe(actual, $"значение переменной \"{varName}\":\"{actual}\" равно \"{expected}\"");
        }

        /// <summary>
        /// Шаг проверки того, что значение переменной содержит строку.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="expected">Expected значение.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" содержит ""(.+)""")]
        public void CheckVariableContains(string varName, string expected)
        {
            expected.Should().NotBeNull($"значение \"expected\" не задано");
            expected = this.variableController.ReplaceVariables(expected) ?? expected;

            var actual = this.variableController.GetVariableValueText(varName);
            actual.Should().NotBeNull($"значения в переменной \"{varName}\" нет");
            actual.Should().Contain(expected, $"значение переменной \"{varName}\":\"{actual}\" не содержит \"{expected}\"");
        }

        /// <summary>
        /// Шаг проверки того, что значение переменной не содержит строку.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="expected">Expected значение.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" не содержит ""(.+)""")]
        public void CheckVariableNotContains(string varName, string expected)
        {
            expected.Should().NotBeNull($"значение \"expected\" не задано");
            expected = this.variableController.ReplaceVariables(expected) ?? expected;

            var actual = this.variableController.GetVariableValueText(varName);
            actual.Should().NotBeNull($"значения в переменной \"{varName}\" нет");
            actual.Should().NotContain(expected, $"значение переменной \"{varName}\":\"{actual}\" содержит \"{expected}\"");
        }

        /// <summary>
        /// Шаг проверки того, что значение переменной начинается со строки.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="expected">Expected значение.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" начинается с ""(.+)""")]
        public void CheckVariableStartsWith(string varName, string expected)
        {
            expected.Should().NotBeNull($"значение \"expected\" не задано");
            expected = this.variableController.ReplaceVariables(expected) ?? expected;

            var actual = this.variableController.GetVariableValueText(varName);
            actual.Should().NotBeNull($"значения в переменной \"{varName}\" нет");
            actual.Should().StartWith(expected, $"значение переменной \"{varName}\":\"{actual}\" не начинается с \"{expected}\"");
        }

        /// <summary>
        /// Шаг проверки того, что значение переменной закачивается строкой.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="expected">Expected значение.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" заканчивается с ""(.+)""")]
        public void CheckVariableEndsWith(string varName, string expected)
        {
            expected.Should().NotBeNull($"значение \"expected\" не задано");
            expected = this.variableController.ReplaceVariables(expected) ?? expected;

            var actual = this.variableController.GetVariableValueText(varName);
            actual.Should().NotBeNull($"значения в переменной \"{varName}\" нет");
            actual.Should().EndWith(expected, $"значение переменной \"{varName}\":\"{actual}\" не заканчивается с \"{expected}\"");
        }
    }
}