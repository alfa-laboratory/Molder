using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using System.Xml.Linq;
using FluentAssertions;
using Molder.Controllers;
using Molder.Helpers;
using TechTalk.SpecFlow;
using Microsoft.Extensions.Logging;
using Molder.Extensions;
using Molder.Generator.Extensions;

namespace Molder.Generator.Steps
{
    /// <summary>
    /// Общие шаги для работы с переменными.
    /// </summary>
    [Binding]
    public class VariableSteps
    {
        private readonly VariableController variableController;
        private readonly Dictionary<string, Type> variablesType;
        /// <summary>
        /// Initializes a new instance of the <see cref="Generator"/> class.
        /// Привязка общих шагов к работе с переменным через контекст.
        /// </summary>
        /// <param name="variableController">Контекст для работы с переменными.</param>
        public VariableSteps(VariableController variableController)
        {
            this.variableController = variableController;
            variablesType = new Dictionary<string, Type>()
            {
                { "int", typeof(int)},
                { "string", typeof(string)},
                { "double", typeof(double)},
                { "bool", typeof(bool)},
                { "object",typeof(object)},
                { "long",typeof(long)},
                { "float",typeof(float)}
            };
        }

        [StepArgumentTransformation]
        public IEnumerable<object> TransformationTableToEnumerable(Table table)
        {
            return table.ToEnumerable(variableController);
        }

        [StepArgumentTransformation]
        public Dictionary<string,object> TransformationTableToDictionary(Table table)
        {
            return table.ToDictionary(variableController);
        }

        [StepArgumentTransformation]
        public TypeCode TransformationTableToEnumerable(string type)
        {
            type.Should().NotBeNull("Значение \"type\" не задано");
            type = type.ToLower();
            if (!variablesType.TryGetValue(type, out Type value)) throw new Exception($"There is no type \"{type}\"");
            return Type.GetTypeCode(value);
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
            this.variableController.Variables.TryRemove(varName, out _);
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
            var str = variableController.ReplaceVariables(text);
            Log.Logger().LogDebug($"Replaced text with variables is equal to {Environment.NewLine}{str}");
            this.variableController.SetVariable(varName, typeof(string), str);
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
            var str = variableController.ReplaceVariables(text);

            Log.Logger().LogDebug(str.TryParseToXml()
                ? $"Replaced multiline text with variables is equal to {Environment.NewLine}{Converter.CreateXMLEscapedString(str)}"
                : $"Replaced multiline text with variables is equal to {Environment.NewLine}{str}");

            this.variableController.SetVariable(varName, typeof(string), str);
        }

        /// <summary>
        /// Шаг для сохранения числа в переменную (float, int).
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="number">Число.</param>
        [StepDefinition(@"я сохраняю число ""(.+)"" в переменную ""(.*)""")]
        public void StoreAsVariableNumber(string number, string varName)
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

            Log.Logger().LogInformation($"input xml is:{Environment.NewLine}{Converter.CreateXMLEscapedString(xmlBody)}");

            var doc = Converter.CreateXmlDoc(xmlBody);
            doc.Should().NotBeNull($"создать XmlDoc из строки {Environment.NewLine}\"{Converter.CreateXMLEscapedString(xmlBody)}\" не удалось");

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
            var value = this.variableController.GetVariableValue(varName);
            value.Should().NotBeNull($"значения в переменной \"{varName}\" нет");

            this.variableController.SetVariable(newVarName, value.GetType(), value);
        }

        /// <summary>
        /// Шаг для сохранения содержимого (в виде текста) одной переменной в другую.
        /// </summary>
        /// <param name="varName">Исходная переменная.</param>
        /// <param name="newVarName">Переменная-результат.</param>
        [StepDefinition(@"я сохраняю содержимое переменной ""(.+)"" в переменную ""(.+)""")]
        public void StoreVariableTextToVariable(string varName, string newVarName)
        {
            var value = this.variableController.GetVariableValueText(varName);
            value.Should().NotBeNull($"содержимого в переменной \"{varName}\" нет");

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
            var value = (string)this.variableController.GetVariableValue(varName);
            value.Should().NotBeNull($"значения в переменной \"{varName}\" нет");
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

            Log.Logger().LogInformation($"Result text is equal to {Environment.NewLine}{replacement}");
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

        /// <summary>
        /// Шаг для сохранения коллекции в переменную без указания типа.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="collection">Коллекция.</param>
        [StepDefinition(@"я сохраняю коллекцию в переменную ""(.+)"":")]
        public void StoreEnumerableAsVariableNoType(string varName, IEnumerable<object> collection)
        {
            varName.Should().NotBeNull("Значение \"varname\" не задано");
            this.variableController.SetVariable(varName, collection.GetType(), collection);
        }

        /// <summary>
        /// Шаг для сохранения коллекции в переменную c указанием типа.
        /// </summary>
        /// <param name="varType">Идентификатор типа переменной.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="collection">Коллекция.</param>
        [StepDefinition(@"я сохраняю коллекцию с типом ""(.+)"" в переменную ""(.+)"":")]
        public void StoreEnumerableAsVariableWithType(TypeCode varType, string varName, IEnumerable<object> collection)
        {
            varType.Should().NotBeNull("Значение \"varType\" не задано");
            varName.Should().NotBeNull("Значение \"varName\" не задано");
            switch (varType) {
                case TypeCode.Object:
                    var tmpParserObject = collection.TryParse<object>();
                    this.variableController.SetVariable(varName, tmpParserObject.GetType(), tmpParserObject);
                    break;
                case TypeCode.Int32:
                    var tmpParserInt = collection.TryParse<int>();
                    this.variableController.SetVariable(varName, tmpParserInt.GetType(), tmpParserInt);
                    break;
                case TypeCode.Boolean:
                    var tmpParserBool = collection.TryParse<bool>();
                    this.variableController.SetVariable(varName, tmpParserBool.GetType(), tmpParserBool);
                    break;
                case TypeCode.String:
                    var tmpParserString = collection.TryParse<string>();
                    this.variableController.SetVariable(varName, tmpParserString.GetType(), tmpParserString);
                    break;
                case TypeCode.Double:
                    var tmpParserStringDouble = collection.TryParse<double>();
                    this.variableController.SetVariable(varName, tmpParserStringDouble.GetType(), tmpParserStringDouble);
                    break;
                case TypeCode.Single:
                    var tmpParserStringFloat = collection.TryParse<float>();
                    this.variableController.SetVariable(varName, tmpParserStringFloat.GetType(), tmpParserStringFloat);
                    break;
                case TypeCode.Int64:
                    var tmpParserStringLong = collection.TryParse<long>();
                    this.variableController.SetVariable(varName, tmpParserStringLong.GetType(), tmpParserStringLong);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Шаг для сохранения произвольного значения из коллекции в переменную.
        /// </summary>
        /// <param name="collectionName">Идентификатор коллекции.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я выбираю произвольное значение из коллекции ""(.+)"" и записываю его в переменную ""(.+)""")]
        public void StoreRandomVariableFromEnumerable(string collectionName, string varName)
        {
            collectionName.Should().NotBeNull("Значение \"collectionName\" не задано");
            varName.Should().NotBeNull("Значение \"varName\" не задано");
            var tmpCollection = this.variableController.GetVariableValue(collectionName);
            tmpCollection.Should().NotBeNull($"значения в переменной \"{collectionName}\" нет");
            tmpCollection.GetType().Name.Should().Be("List`1", $"\"{collectionName}\" не является коллекцией");
            switch (tmpCollection.GetType().ToString())
            {
                case "System.Collections.Generic.List`1[System.Object]":
                    var var1 = ((List<object>)tmpCollection).GetRandomValueFromEnumerable();
                    this.variableController.SetVariable(varName, var1.GetType(), var1);
                    break;
                case "System.Collections.Generic.List`1[System.Int32]":
                    var1 = ((List<int>)tmpCollection).GetRandomValueFromEnumerable();
                    this.variableController.SetVariable(varName, var1.GetType(), var1);
                    break;
                case "System.Collections.Generic.List`1[System.Boolean]":
                    var1 = ((List<bool>)tmpCollection).GetRandomValueFromEnumerable();
                    this.variableController.SetVariable(varName, var1.GetType(), var1);
                    break;
                case "System.Collections.Generic.List`1[System.Double]":
                    var1 = ((List<double>)tmpCollection).GetRandomValueFromEnumerable();
                    this.variableController.SetVariable(varName, var1.GetType(), var1);
                    break;
                case "System.Collections.Generic.List`1[System.String]":
                    var1 = ((List<string>)tmpCollection).GetRandomValueFromEnumerable();
                    this.variableController.SetVariable(varName, var1.GetType(), var1);
                    break;
                case "System.Collections.Generic.List`1[System.Single]":
                    var1 = ((List<float>)tmpCollection).GetRandomValueFromEnumerable();
                    this.variableController.SetVariable(varName, var1.GetType(), var1);
                    break;
                case "System.Collections.Generic.List`1[System.Int64]":
                    var1 = ((List<long>)tmpCollection).GetRandomValueFromEnumerable();
                    this.variableController.SetVariable(varName, var1.GetType(), var1);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Шаг для сохранения значения из коллекции в переменную.
        /// </summary>
        /// <param name="collectionName">Идентификатор коллекции.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я выбираю значение из коллекции ""(.+)"" с номером ""(.+)"" и записываю его в переменную ""(.+)""")]
        public void StoreVariableFromEnumerable(string collectionName, string number, string varName)
        {
            collectionName.Should().NotBeNull("Значение \"collectionName\" не задано");
            varName.Should().NotBeNull("Значение \"varName\" не задано");
            number.Should().NotBeNull("Значение \"number\" не задано");
            if (!Int32.TryParse(number, out int tmpNumber))
            {
                throw new Exception($"значение \"number\" не является числом");
            }
            var tmpCollection = this.variableController.GetVariableValue(collectionName);
            tmpCollection.Should().NotBeNull($"значения в переменной \"{collectionName}\" нет");
            tmpCollection.GetType().Name.Should().Be("List`1", $"\"{collectionName}\" не является коллекцией");
            
            switch (tmpCollection.GetType().ToString()) {
                case "System.Collections.Generic.List`1[System.Object]":
                    ((List<object>)tmpCollection).Count.Should().BeGreaterThan(tmpNumber, 
                        "\"number\" должно быть меньше количества записей в \"collectionName\"");
                    var var1 = ((List<object>)tmpCollection).GetValueFromEnumerable<object>(tmpNumber);
                    this.variableController.SetVariable(varName, var1.GetType(), var1);
                    break;
                case "System.Collections.Generic.List`1[System.Int32]":
                    ((List<int>)tmpCollection).Count.Should().BeGreaterThan(tmpNumber,
                        "\"number\" должно быть меньше количества записей в \"collectionName\"");
                    var1 = ((List<int>)tmpCollection).GetValueFromEnumerable<int>(tmpNumber);
                    this.variableController.SetVariable(varName, var1.GetType(), var1);
                    break;
                case "System.Collections.Generic.List`1[System.Boolean]":
                    ((List<bool>)tmpCollection).Count.Should().BeGreaterThan(tmpNumber,
                        "\"number\" должно быть меньше количества записей в \"collectionName\"");
                    var1 = ((List<bool>)tmpCollection).GetValueFromEnumerable<bool>(tmpNumber);
                    this.variableController.SetVariable(varName, var1.GetType(), var1);
                    break;
                case "System.Collections.Generic.List`1[System.Double]":
                    ((List<double>)tmpCollection).Count.Should().BeGreaterThan(tmpNumber,
                        "\"number\" должно быть меньше количества записей в \"collectionName\"");
                    var1 = ((List<double>)tmpCollection).GetValueFromEnumerable<double>(tmpNumber);
                    this.variableController.SetVariable(varName, var1.GetType(), var1);
                    break;
                case "System.Collections.Generic.List`1[System.String]":
                    ((List<string>)tmpCollection).Count.Should().BeGreaterThan(tmpNumber,
                        "\"number\" должно быть меньше количества записей в \"collectionName\"");
                    var1 = ((List<string>)tmpCollection).GetValueFromEnumerable<string>(tmpNumber);
                    this.variableController.SetVariable(varName, var1.GetType(), var1);
                    break;
                case "System.Collections.Generic.List`1[System.Single]":
                    ((List<float>)tmpCollection).Count.Should().BeGreaterThan(tmpNumber,
                        "\"number\" должно быть меньше количества записей в \"collectionName\"");
                    var1 = ((List<float>)tmpCollection).GetValueFromEnumerable<float>(tmpNumber);
                    this.variableController.SetVariable(varName, var1.GetType(), var1);
                    break;
                case "System.Collections.Generic.List`1[System.Int64]":
                    ((List<long>)tmpCollection).Count.Should().BeGreaterThan(tmpNumber,
                        "\"number\" должно быть меньше количества записей в \"collectionName\"");
                    var1 = ((List<long>)tmpCollection).GetValueFromEnumerable<long>(tmpNumber);
                    this.variableController.SetVariable(varName, var1.GetType(), var1);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Шаг для сохранения словаря в переменную без указания типа.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="dictionary">Словарь.</param>
        [StepDefinition(@"я сохраняю словарь в переменную ""(.+)"":")]
        public void StoreDictionaryAsVariableNoType(string varName, Dictionary<string,object> dictionary)
        {
            varName.Should().NotBeNull("Значение \"varname\" не задано");
            this.variableController.SetVariable(varName, dictionary.GetType(), dictionary);
        }

        /// <summary>
        /// Шаг для сохранения произвольного значения из словаря в переменную.
        /// </summary>
        /// <param name="dictionaryName">Идентификатор словаря.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я выбираю произвольное значение из словаря ""(.+)"" и записываю его в переменную ""(.+)""")]
        public void StoreRandomVariableFromDictionary(string dictionaryName, string varName)
        {
            varName.Should().NotBeNull("Значение \"varname\" не задано");
            dictionaryName.Should().NotBeNull("Значение \"dictionaryName\" не задано");
            var var1 = ((Dictionary<string, object>)this.variableController.GetVariableValue(dictionaryName)).GetRandomValueFromDictionary();
            this.variableController.SetVariable(varName, var1.GetType(), var1);
        }

        /// <summary>
        /// Шаг для сохранения значения из коллекции в переменную.
        /// </summary>
        /// <param name="dictionaryName">Идентификатор словаря.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я выбираю значение из словаря ""(.+)"" с ключом ""(.+)"" и записываю его в переменную ""(.+)""")]
        public void StoreRandomVariableFromDictionary(string dictionaryName, string key, string varName)
        {
            dictionaryName.Should().NotBeNull("Значение \"dictionaryName\" не задано");
            key.Should().NotBeNull("Значение \"key\" не задано");
            varName.Should().NotBeNull("Значение \"varName\" не задано");
            key = variableController.ReplaceVariables(key) ?? key;
            var var1 = ((Dictionary<string, object>)this.variableController.GetVariableValue(dictionaryName)).GetValueFromDictionary(key);
            var1.Should().NotBeNull($"В словаре {dictionaryName} нет записи с ключом {key}");
            this.variableController.SetVariable(varName, var1.GetType(), var1);
        }
    }
}