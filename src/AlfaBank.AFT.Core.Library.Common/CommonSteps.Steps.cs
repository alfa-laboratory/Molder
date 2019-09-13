// <copyright file="CommonSteps.Steps.cs" company="AlfaBank">
// Copyright (c) AlfaBank. All rights reserved.
// </copyright>

namespace AlfaBank.AFT.Core.Library.Common
{
    using System;
    using System.Net;
    using System.Threading;
    using AlfaBank.AFT.Core.Helpers;
    using AlfaBank.AFT.Core.Infrastructure.Common;
    using AlfaBank.AFT.Core.Model.Context;
    using FluentAssertions;
    using TechTalk.SpecFlow;
    using Xunit.Abstractions;

    /// <summary>
    /// Общие шаги для генерации данных, работы с переменными, отладки.
    /// </summary>
    [Binding]
    public class CommonSteps
    {
        private readonly VariableContext variableContext;
        private readonly ITestOutputHelper consoleOutputHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonSteps"/> class.
        /// Привязка общих шагов к работе с переменным через контекст.
        /// </summary>
        /// <param name="variableContext">Контекст для работы с переменными.</param>
        /// <param name="consoleOutputHelper">Capturing Output.</param>
        public CommonSteps(VariableContext variableContext, ITestOutputHelper consoleOutputHelper)
        {
            this.variableContext = variableContext;
            this.consoleOutputHelper = consoleOutputHelper;
        }

        /// <summary>
        /// Шаг для отображения типа значения в переменной.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"ОТЛАДКА: показать тип переменной ""(.+)""")]
        public void DisplayVariableType(string varName)
        {
            this.variableContext.Should().NotBeNull("Контекст не создан");
            var type = this.variableContext.GetVariableValue(varName).GetType();
            type.Should().NotBeNull($"У переменной '{varName}' нет типа");
            this.consoleOutputHelper.WriteLine($"[DEBUG] VARIABLE TYPE: {type.FullName}");
        }

        /// <summary>
        /// Шаг для отображения значения переменной.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"ОТЛАДКА: показать значение переменной ""(.+)""")]
        public void DisplayVariableValue(string varName)
        {
            this.variableContext.Should().NotBeNull("Контекст не создан");
            var value = this.variableContext.GetVariableValueText(varName);
            value.Should().NotBeNull($"У переменной '{varName}' значение не найдено");
            this.consoleOutputHelper.WriteLine($"[DEBUG] VARIABLE VALUE: {value}");
        }

        /// <summary>
        /// Шаг для шифрования текста с открытым ключем.
        /// </summary>
        /// <param name="salt">Ключ шифрования.</param>
        /// <param name="text">Текст для защифровки.</param>
        [StepDefinition(@"ОТЛАДКА: зашифровать текст с ключем ""(.*)"":")]
        public void EncodeText(string salt, string text)
        {
            salt.Should().NotBeEmpty("Ксюч шифрование не может быть пустой строкой");
            text.Should().NotBeEmpty("Текст для зашифровки не может быть пустой строкой");
            var enc = new Encryptor(string.IsNullOrEmpty(salt) ? null : salt);
            this.consoleOutputHelper.WriteLine($"[DEBUG] ENCODED TEXT: {enc.Encrypt(text)}");
        }

        /// <summary>
        /// Шаг для явного ожидания.
        /// </summary>
        /// <param name="seconds">Количество секунд ожидания.</param>
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
            this.variableContext.Variables.ContainsKey(varName).Should().BeTrue($"Переменной '{varName}' не существует");
            this.variableContext.Variables.Remove(varName);
        }

        /// <summary>
        /// Шаг для очистки значения переменной.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я очищаю переменную ""(.+)""")]
        public void EmtpyVariable(string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeTrue($"Переменной '{varName}' не существует");
            this.variableContext.SetVariable(varName, typeof(object), null);
        }

        /// <summary>
        /// Шаг для сохранения значения однострочного текста в переменную.
        /// </summary>
        /// <param name="text">Текст для сохранения в переменную.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю текст ""(.*)"" в переменную ""(.+)""")]
        public void StoreAsVariableString(string text, string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            this.variableContext.SetVariable(varName, typeof(string), text);
        }

        /// <summary>
        /// Шаг для сохранения многострочного текста в переменную.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="text">Текст для сохранения в переменную.</param>
        [StepDefinition(@"я сохраняю текст в переменную ""(.+)"":")]
        public void StoreAsVariableText(string varName, string text)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            this.variableContext.SetVariable(varName, typeof(string), text);
        }

        /// <summary>
        /// Шаг для сохранения числа в переменную (float, int).
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="number">Число.</param>
        [StepDefinition(@"я сохраняю число ""(.+)"" в переменную ""(.*)""")]
        public void StoreAsVariableNumber(string varName, string number)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            if (decimal.TryParse(number, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.CurrentCulture, out var dec))
            {
                this.variableContext.SetVariable(varName, typeof(decimal), dec);
                return;
            }

            if (decimal.TryParse(number, System.Globalization.NumberStyles.Float, new System.Globalization.NumberFormatInfo() { PercentDecimalSeparator = ".", CurrencyDecimalSeparator = ".", NumberDecimalSeparator = "." }, out dec))
            {
                this.variableContext.SetVariable(varName, typeof(decimal), dec);
                return;
            }

            if (decimal.TryParse(number, System.Globalization.NumberStyles.Float, new System.Globalization.NumberFormatInfo() { PercentDecimalSeparator = ",", CurrencyDecimalSeparator = ",", NumberDecimalSeparator = "," }, out dec))
            {
                this.variableContext.SetVariable(varName, typeof(decimal), dec);
                return;
            }

            this.variableContext.SetVariable(varName, typeof(int), int.Parse(number));
        }

        /// <summary>
        /// Шаг для сохранения XML документа в переменную.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="xml">Текст, представленный в виде XML.</param>
        [StepDefinition(@"я сохраняю текст как XML документ в переменную ""(.+)"":")]
        public void StoreAsVariableXmlFromText(string varName, string xml)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            xml = this.variableContext.ReplaceVariablesInXmlBody(xml);
            var doc = System.Xml.Linq.XDocument.Parse(xml);
            this.variableContext.SetVariable(varName, doc.GetType(), doc);
        }

        /// <summary>
        /// Шаг для сохранения даты в переменную.
        /// </summary>
        /// <param name="day">День.</param>
        /// <param name="month">Месяц.</param>
        /// <param name="year">Год.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю дату ([0-9]{1,2})\.([0-9]{2})\.([0-9]+) в переменную ""(.+)""")]
        public void StoreAsVariableDate(int day, int month, int year, string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var dt = new DateTime(year, month, day);
            this.variableContext.SetVariable(varName, dt.GetType(), dt);
        }

        /// <summary>
        /// Шаг для сохранения даты в переменную, используя конкретный формат.
        /// </summary>
        /// <param name="day">День.</param>
        /// <param name="month">Месяц.</param>
        /// <param name="year">Год.</param>
        /// <param name="format">Формат представления даты.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю дату ([0-9]{1,2})\.([0-9]{2})\.([0-9]+) в формате ""(.+)"" в переменную ""(.+)""")]
        public void StoreAsVariableDateWithFormat(int day, int month, int year, string format, string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var dt = new DateTime(year, month, day).ToString(format);
            this.variableContext.SetVariable(varName, dt.GetType(), dt);
        }

        /// <summary>
        /// Шаг для сохранения времени в переменную.
        /// </summary>
        /// <param name="hours">Часы.</param>
        /// <param name="minutes">Минуты.</param>
        /// <param name="seconds">Секунды.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю время ([0-9]{1,2}):([0-9]{2}):([0-9]{2}) в переменную ""(.+)""")]
        public void StoreAsVariableTime(int hours, int minutes, int seconds, string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var dt = new DateTime(1, 1, 1, hours, minutes, seconds);
            this.variableContext.SetVariable(varName, dt.GetType(), dt);
        }

        /// <summary>
        /// Шаг для сохранения времения в переменную, используя конкретный формат.
        /// </summary>
        /// <param name="hours">Часы.</param>
        /// <param name="minutes">Минуты.</param>
        /// <param name="seconds">Секунды.</param>
        /// <param name="format">Формат представления времени.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю время ([0-9]{1,2}):([0-9]{2}):([0-9]{2}) в формате ""(.+)"" в переменную ""(.+)""")]
        public void StoreAsVariableTimeWithFormat(int hours, int minutes, int seconds, string format, string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var dt = new DateTime(1, 1, 1, hours, minutes, seconds).ToString(format);
            this.variableContext.SetVariable(varName, dt.GetType(), dt);
        }

        /// <summary>
        /// Шаг для сохранения точного времени в переменную (с миллисекундами).
        /// </summary>
        /// <param name="hours">Часы.</param>
        /// <param name="minutes">Минуты.</param>
        /// <param name="seconds">Секунды.</param>
        /// <param name="milliseconds">Миллисекунды.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю время ([0-9]{1,2}):([0-9]{2}):([0-9]{2})\.([0-9]+) в переменную ""(.+)""")]
        public void StoreAsVariableTimeLong(int hours, int minutes, int seconds, int milliseconds, string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var dt = new DateTime(1, 1, 1, hours, minutes, seconds, milliseconds);
            this.variableContext.SetVariable(varName, dt.GetType(), dt);
        }

        /// <summary>
        /// Шаг для сохранения точного времени (с миллисекундами) в переменную, используя конкретный формат.
        /// </summary>
        /// <param name="hours">Часы.</param>
        /// <param name="minutes">Минуты.</param>
        /// <param name="seconds">Секунды.</param>
        /// <param name="milliseconds">Миллисекунды.</param>
        /// <param name="format">Формат представления времени.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю время ([0-9]{1,2}):([0-9]{2}):([0-9]{2})\.([0-9]+) в формате ""(.+)"" в переменную ""(.+)""")]
        public void StoreAsVariableTimeLongWithFormat(int hours, int minutes, int seconds, int milliseconds, string format, string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var dt = new DateTime(1, 1, 1, hours, minutes, seconds, milliseconds).ToString(format);
            this.variableContext.SetVariable(varName, dt.GetType(), dt);
        }

        /// <summary>
        /// Шаг для сохранения даты и времени в переменную.
        /// </summary>
        /// <param name="day">День.</param>
        /// <param name="month">Месяц.</param>
        /// <param name="year">Год.</param>
        /// <param name="hours">Часы.</param>
        /// <param name="minutes">Минуты.</param>
        /// <param name="seconds">Секунды.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю дату и время ([0-9]{1,2})\.([0-9]{2})\.([0-9]+) ([0-9]{1,2}):([0-9]{2}):([0-9]{2}) в переменную ""(.+)""")]
        public void StoreAsVariableDateTime(int day, int month, int year, int hours, int minutes, int seconds, string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var dt = new DateTime(year, month, day, hours, minutes, seconds);
            this.variableContext.SetVariable(varName, dt.GetType(), dt);
        }

        /// <summary>
        /// Шаг для сохранения даты и времены в переменную, используя конкретный формат.
        /// </summary>
        /// <param name="day">День.</param>
        /// <param name="month">Месяц.</param>
        /// <param name="year">Год.</param>
        /// <param name="hours">Часы.</param>
        /// <param name="minutes">Минуты.</param>
        /// <param name="seconds">Секунды.</param>
        /// <param name="format">Формат представления даты и времени.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю дату и время ([0-9]{1,2})\.([0-9]{2})\.([0-9]+) ([0-9]{1,2}):([0-9]{2}):([0-9]{2}) в формате ""(.+)"" в переменную ""(.+)""")]
        public void StoreAsVariableDateTimeWithFormat(int day, int month, int year, int hours, int minutes, int seconds, string format, string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var dt = new DateTime(year, month, day, hours, minutes, seconds).ToString(format);
            this.variableContext.SetVariable(varName, dt.GetType(), dt);
        }

        /// <summary>
        /// Шаг для сохранения даты и точного времени (с миллисекундами) в переменную.
        /// </summary>
        /// <param name="day">День.</param>
        /// <param name="month">Месяц.</param>
        /// <param name="year">Год.</param>
        /// <param name="hours">Часы.</param>
        /// <param name="minutes">Минуты.</param>
        /// <param name="seconds">Секунды.</param>
        /// <param name="milliseconds">Миллисекунды.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю дату и время ([0-9]{1,2})\.([0-9]{2})\.([0-9]+) ([0-9]{1,2}):([0-9]{2}):([0-9]{2})\.([0-9]+) в переменную ""(.+)""")]
        public void StoreAsVariableOtherDateTime(int day, int month, int year, int hours, int minutes, int seconds, int milliseconds, string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var dt = new DateTime(year, month, day, hours, minutes, seconds, milliseconds);
            this.variableContext.SetVariable(varName, dt.GetType(), dt);
        }

        /// <summary>
        /// Шаг для сохранения даты и точного времени (с миллисекундами) в переменную, используя конкретный формат.
        /// </summary>
        /// <param name="day">День.</param>
        /// <param name="month">Месяц.</param>
        /// <param name="year">Год.</param>
        /// <param name="hours">Часы.</param>
        /// <param name="minutes">Минуты.</param>
        /// <param name="seconds">Секунды.</param>
        /// <param name="milliseconds">Миллисекунды.</param>
        /// <param name="format">Формат представления даты и времени.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю дату и время ([0-9]{1,2})\.([0-9]{2})\.([0-9]+) ([0-9]{1,2}):([0-9]{2}):([0-9]{2})\.([0-9]+) в формате ""(.+)"" в переменную ""(.+)""")]
        public void StoreAsVariableOtherDateTimeWithFormat(int day, int month, int year, int hours, int minutes, int seconds, int milliseconds, string format, string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var dt = new DateTime(year, month, day, hours, minutes, seconds, milliseconds).ToString(format);
            this.variableContext.SetVariable(varName, dt.GetType(), dt);
        }

        /// <summary>
        /// Шаг для сохранения текущего времени в переменную.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю текущее время в переменную ""(.+)""")]
        public void StoreAsVariableCurrentTime(string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var now = new DateTime(DateTime.Now.TimeOfDay.Ticks);
            var dt = new DateTime(1, 1, 1, now.Hour, now.Minute, now.Second, now.Millisecond);
            this.variableContext.SetVariable(varName, dt.GetType(), dt);
        }

        /// <summary>
        /// Шаг для сохранения текущей даты в переменную.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю текущую дату в переменную ""(.+)""")]
        public void StoreAsVariableCurrentDate(string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var dt = DateTime.Now.Date;
            this.variableContext.SetVariable(varName, dt.GetType(), dt);
        }

        /// <summary>
        /// Шаг для сохранения текущей даты в переменную, используя конкретный формат.
        /// </summary>
        /// <param name="format">Формат представления даты.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю текущую дату в формате '(.+)' в переменную ""(.+)""")]
        public void StoreAsVariableCurrentDateWithFormat(string format, string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var dt = DateTime.Now.Date.ToString(format);
            this.variableContext.SetVariable(varName, dt.GetType(), dt);
        }

        /// <summary>
        /// Шаг для сохранения текущей даты и времени в переменную.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю текущие дату и время в переменную ""(.+)""")]
        public void StoreAsVariableCurrentDateTime(string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var dt = DateTime.Now;
            this.variableContext.SetVariable(varName, dt.GetType(), dt);
        }

        /// <summary>
        /// Шаг для сохранения даты, которая отличается от текущей на определенный срок в переменную, используя конкретный формат.
        /// </summary>
        /// <param name="year">Количество лет от текущей даты.</param>
        /// <param name="month">Количество месяцев от текущей даты.</param>
        /// <param name="day">Количество дней от текущей даты.</param>
        /// <param name="format">Формат представления даты.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю будущую дату, которая отличается от текущей на ""([0-9]+)"" (?:лет|год[а]?) ""([0-9]+)"" (?:месяц|месяц(?:а|ев)) ""([0-9]+)"" (?:день|дн(?:я|ей)) в формате ""(.+)"" в переменную ""(.+)""")]
        public void StoreAsVariableFutureDateTimeWithDifference(int year, int month, int day, string format, string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var dt = DateTime.Now.AddYears(year).AddMonths(month).AddDays(day).ToString(format);
            this.variableContext.SetVariable(varName, dt.GetType(), dt);
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
            this.variableContext.Variables.ContainsKey(varName).Should().BeTrue($"Переменной '{varName}' не существует");
            this.variableContext.Variables.ContainsKey(newVarName).Should().BeFalse($"Переменная '{newVarName}' уже существует");
            this.variableContext.GetVariableValue(varName).Should().NotBeNull($"Значения в переменной {varName} нет");

            var replacement = string.Empty;

            if (this.variableContext.Variables.ContainsKey(varName) &&
                this.variableContext.GetVariableValue(varName) != null)
            {
                if (this.variableContext.Variables[varName].Type == typeof(string))
                {
                    replacement = (string)this.variableContext.GetVariableValue(varName);
                }
                else
                {
                    replacement = this.variableContext.GetVariableValue(varName).ToString();
                }
            }

            this.variableContext.SetVariable(newVarName, typeof(string), text?.Replace($"{{{varName}}}", replacement));
        }

        /// <summary>
        /// Шаг для сохранения случанойго набора букв и цифр в переменную, используя конкретный префикс.
        /// </summary>
        /// <param name="len">Длина строки.</param>
        /// <param name="prefix">Префикс.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю случайный набор букв и цифр длиной ([0-9]+) знаков с префиксом ""(.+)"" в переменную ""(.+)""")]
        public void StoreAsVariableRandomStringWithPrefix(int len, string prefix, string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            len.Should().NotBe(0, "Длина строки не может быть 0");
            prefix.Should().NotBeEmpty("Длина префикса не может быть нулевой");
            var str = DataGenerator.GetRandomStringWithPrefix(len, prefix);
            this.variableContext.SetVariable(varName, str.GetType(), str);
        }

        /// <summary>
        /// Шаг для добавления случайного набора букв в переменную, используя конкретный префикс.
        /// </summary>
        /// <param name="len">Длина строки.</param>
        /// <param name="prefix">Префикс.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю случайный набор букв длиной ([0-9]+) знаков с префиксом ""(.+)"" в переменную ""(.+)""")]
        public void StoreAsVariableRandomCharWithPrefix(int len, string prefix, string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            len.Should().NotBe(0, "Длина строки не может быть 0");
            prefix.Should().NotBeEmpty("Длина префикса не может быть нулевой");
            var str = DataGenerator.GetRandomCharWithPrefix(len, prefix);
            this.variableContext.SetVariable(varName, str.GetType(), str);
        }

        /// <summary>
        /// Шаг для добавления случайного набора цифр в переменную, ипользуя конкретный префикс.
        /// </summary>
        /// <param name="len">Длина строки.</param>
        /// <param name="prefix">Префикс.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю случайный набор цифр длиной ([0-9]+) знаков с префиксом ""(.+)"" в переменную ""(.+)""")]
        public void StoreAsVariableRandomNumberWithPrefix(int len, string prefix, string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            len.Should().NotBe(0, "Длина строки не может быть 0");
            prefix.Should().NotBeEmpty("Длина префикса не может быть нулевой");
            var str = DataGenerator.GetRandomNumberWithPrefix(len, prefix);
            this.variableContext.SetVariable(varName, str.GetType(), str);
        }

        /// <summary>
        /// Шаг для добавления случайного набора букв и цифр в переменную.
        /// </summary>
        /// <param name="len">Длина строки.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю случайный набор букв и цифр длиной ([0-9]+) знаков в переменную ""(.+)""")]
        public void StoreAsVariableRandomString(int len, string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            len.Should().NotBe(0, "Длина строки не может быть 0");
            var str = DataGenerator.GetRandomString(len);
            this.variableContext.SetVariable(varName, str.GetType(), str);
        }

        /// <summary>
        /// Шаг для добавления случайного набора букв в переменную.
        /// </summary>
        /// <param name="len">Длина строки.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю случайный набор букв длиной ([0-9]+) знаков в переменную ""(.+)""")]
        public void StoreAsVariableRandomChar(int len, string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            len.Should().NotBe(0, "Длина строки не может быть 0");
            var str = DataGenerator.GetRandomChars(len);
            this.variableContext.SetVariable(varName, str.GetType(), str);
        }

        /// <summary>
        /// Шаг для добавления случайного набора цифр в переменную.
        /// </summary>
        /// <param name="len">Длина строки.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю случайный набор цифр длиной ([0-9]+) знаков в переменную ""(.+)""")]
        public void StoreAsVariableRandomNumber(int len, string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            len.Should().NotBe(0, "Длина строки не может быть 0");
            var str = DataGenerator.GetRandomNumbers(len);
            this.variableContext.SetVariable(varName, str.GetType(), str);
        }

        /// <summary>
        /// Шаг для сохранения случайного номера телефона в переменную, используя конкретный формат.
        /// Пример формата: 7XXXXXXXXX.
        /// </summary>
        /// <param name="mask">Маска для телефона.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю случайный номер телефона в формате ""(.+)"" в переменную ""(.+)""")]
        public void StoreAsVariableRandomPhone(string mask, string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            mask.Length.Should().NotBe(0, "Длина строки не может быть 0");
            var str = DataGenerator.GetRandomPhone(mask);
            this.variableContext.SetVariable(varName, str.GetType(), str);
        }

        /// <summary>
        /// Шаг для сохранения UUID в переменную.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю новый (универсальный уникальный идентификатор|UUID) в переменную ""(.+)""")]
        public void StoreAsVariableUuid(string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var str = DataGenerator.GetGuid();
            this.variableContext.SetVariable(varName, str.GetType(), str);
        }

        /// <summary>
        /// Шаг для сохранения значения одной переменной в другую.
        /// </summary>
        /// <param name="varName">Исходная переменная.</param>
        /// <param name="newVarName">Переменная-результат.</param>
        [StepDefinition(@"я сохраняю значение переменной ""(.+)"" в переменную ""(.+)""")]
        public void StoreVariableValueToVariable(string varName, string newVarName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeTrue($"Переменной '{varName}' не существует");
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{newVarName}' уже существует");
            var value = this.variableContext.GetVariableValue(varName);
            value.Should().NotBeNull($"Значения в переменной {varName} нет");

            this.variableContext.SetVariable(newVarName, value.GetType(), value);
        }

        /// <summary>
        /// Шаг для сохранения Credentials в переменную.
        /// </summary>
        /// <param name="host">Хост.</param>
        /// <param name="authType">Тип авторизации.</param>
        /// <param name="domain">Домен.</param>
        /// <param name="username">Логин.</param>
        /// <param name="password">Зашифрованный пароль.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я создаю полномочия для хоста ""(.+)"" c типом ""(.+)"" для пользователя с доменом ""(.+)"", логином ""(.+)"", паролем ""(.+)"" и сохраняю в переменную ""(.+)""")]
        public void StoreCredentialsForHostToVariable(string host, AuthType authType, string domain, string username, string password, string varName)
        {
            var credentialCache = new CredentialCache();
            var networkCredential = new NetworkCredential(username, new Encryptor().Decrypt(password), domain);
            credentialCache.Add(new Uri(host), authType.ToString(), networkCredential);
            this.variableContext.SetVariable(varName, credentialCache.GetType(), credentialCache);
        }

        /// <summary>
        /// Шаг проверки, что значение переменной не является NULL.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" не является NULL")]
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" существует")]
        public void CheckVariableIsNotNull(string varName)
        {
            var value = this.variableContext.GetVariableValue(varName);
            value.Should().NotBeNull($"Значение переменной '{varName}' является NULL");
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
            var value = this.variableContext.GetVariableValue(varName);
            value.Should().BeNull($"Значение переменной '{varName}' не является NULL");
        }

        /// <summary>
        /// Шаг проверки, что значение переменной не является пустой строкой.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" не является пустой строкой")]
        public void CheckVariableIsNotEmpty(string varName)
        {
            var value = this.variableContext.GetVariableValue(varName);
            value.Should().NotBeNull($"Значения в переменной {varName} нет");
            if (this.variableContext.GetVariable(varName)?.Type == typeof(string))
            {
                string.IsNullOrEmpty((string)value).Should().BeFalse($"Значение переменной '{varName}' пустая строка");
            }
        }

        /// <summary>
        /// Шаг проверки, что значение переменной  является пустой строкой.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" пустая строка")]
        public void CheckVariableIsEmpty(string varName)
        {
            var value = this.variableContext.GetVariableValue(varName);
            value.Should().NotBeNull($"Значения в переменной {varName} нет");
            if (this.variableContext.GetVariable(varName)?.Type == typeof(string))
            {
                string.IsNullOrEmpty((string)value).Should().BeTrue($"Значение переменной '{varName}' не пустая строка");
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
            expected.Should().NotBeNull($"Значение 'expected' не задано");
            var actual = this.variableContext.GetVariableValueText(varName);
            expected.GetType().Should().Be(actual.GetType(), $"Тип значения переменной '{varName}' не совпадает с типом '{expected}'");
            expected.Should().Be(actual, $"Значение переменной '{varName}':'{actual}' не равно '{expected}'");
        }

        /// <summary>
        /// Шаг проверки, что значение переменной не равно переданному объекту.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="expected">Expected значение.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" не равно ""(.+)""")]
        public void CheckVariableNotEquals(string varName, object expected)
        {
            expected.Should().NotBeNull($"Значение 'expected' не задано");
            var actual = this.variableContext.GetVariableValueText(varName);
            expected.GetType().Should().Be(actual.GetType(), $"Тип значения переменной '{varName}' не совпадает с типом '{expected}'");
            expected.Should().NotBe(actual, $"Значение переменной '{varName}':'{actual}' равно '{expected}'");
        }

        /// <summary>
        /// Шаг проверки того, что значение переменной содержит строку.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="expected">Expected значение.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" содержит ""(.+)""")]
        public void CheckVariableContains(string varName, string expected)
        {
            expected.Should().NotBeNull($"Значение 'expected' не задано");
            var actual = this.variableContext.GetVariableValueText(varName);
            actual?.Contains(expected).Should().BeTrue($"Значение переменной '{varName}':'{actual}' не содержит '{expected}'");
        }

        /// <summary>
        /// Шаг проверки того, что значение переменной не содержит строку.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="expected">Expected значение.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" не содержит ""(.+)""")]
        public void CheckVariableNotContains(string varName, string expected)
        {
            expected.Should().NotBeNull($"Значение 'expected' не задано");
            var actual = this.variableContext.GetVariableValueText(varName);
            actual.Contains(expected).Should().BeFalse($"Значение переменной '{varName}':'{actual}' содержит '{expected}'");
        }

        /// <summary>
        /// Шаг проверки того, что значение переменной начинается со строки.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="expected">Expected значение.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" начинается с ""(.+)""")]
        public void CheckVariableStartsWith(string varName, string expected)
        {
            expected.Should().NotBeNull($"Значение 'expected' не задано");
            var actual = this.variableContext.GetVariableValueText(varName);
            actual.StartsWith(expected).Should().BeTrue($"Значение переменной '{varName}':'{actual}' не начинается с '{expected}'");
        }

        /// <summary>
        /// Шаг проверки того, что значение переменной закачивается строкой.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="expected">Expected значение.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" заканчивается с ""(.+)""")]
        public void CheckVariableEndsWith(string varName, string expected)
        {
            expected.Should().NotBeNull($"Значение 'expected' не задано");
            var actual = this.variableContext.GetVariableValueText(varName);
            actual.EndsWith(expected).Should().BeTrue($"Значение переменной '{varName}':'{actual}' не заканчивается с '{expected}'");
        }

        /// <summary>
        /// Шаг проверки того, что значение одной переменной равно значению другой переменной.
        /// </summary>
        /// <param name="varName1">Переменная 1.</param>
        /// <param name="varName2">Переменная 2.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" равно значению переменной ""(.+)""")]
        public void CheckVariablesAreEqual(string varName1, string varName2)
        {
            var value1 = this.variableContext.GetVariableValueText(varName1);
            var value2 = this.variableContext.GetVariableValueText(varName2);
            value1.Should().Be(
                value2,
                $"Значение переменной '{varName1}':'{value1}' не равно значению переменной '{varName2}':'{value2}'");
        }

        /// <summary>
        /// Шаг проверки того, что значение одной переменной не равно значению другой переменной.
        /// </summary>
        /// <param name="varName1">Переменная 1.</param>
        /// <param name="varName2">Переменная 2.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" не равно значению переменной ""(.+)""")]
        public void CheckVariablesAreNotEqual(string varName1, string varName2)
        {
            var value1 = this.variableContext.GetVariableValueText(varName1);
            var value2 = this.variableContext.GetVariableValueText(varName2);
            value1.Should().NotBe(
                value2,
                $"Значение переменной '{varName1}':'{value1}' равно значению переменной '{varName2}':'{value2}'");
        }

        /// <summary>
        /// Шаг проверки того, что значение одной переменной содержит значение другой переменной.
        /// </summary>
        /// <param name="varName1">Переменная 1.</param>
        /// <param name="varName2">Переменная 2.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" содержит значение переменной ""(.+)""")]
        public void CheckVariableAreContains(string varName1, string varName2)
        {
            var first = this.variableContext.GetVariableValueText(varName1);
            var second = this.variableContext.GetVariableValueText(varName2);

            first.Contains(second).Should().BeTrue($"Значение переменной '{varName1}':'{first}' не содержит значение переменной '{varName2}':'{second}'");
        }

        /// <summary>
        /// Шаг проверки того, что значение одной переменной не содержит значение другой переменной.
        /// </summary>
        /// <param name="varName1">Переменная 1.</param>
        /// <param name="varName2">Переменная 2.</param>
        [Then(@"я убеждаюсь, что значение переменной ""(.+)"" не содержит значение переменной ""(.+)""")]
        public void CheckVariableAreNotContains(string varName1, string varName2)
        {
            var first = this.variableContext.GetVariableValueText(varName1);
            var second = this.variableContext.GetVariableValueText(varName2);

            first.Contains(second).Should().BeFalse($"Значение переменной '{varName1}':'{first}' содержит значение переменной '{varName2}':'{second}'");
        }
    }
}
