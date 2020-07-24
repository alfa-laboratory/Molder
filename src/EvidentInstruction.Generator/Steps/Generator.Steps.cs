using System;
using System.Net;
using FluentAssertions;
using EvidentInstruction.Controllers;
using EvidentInstruction.Infrastructure;
using TechTalk.SpecFlow;
using EvidentInstruction.Generator.Models;

namespace EvidentInstruction.Generator.Steps
{
    /// <summary>
    /// Общие шаги для генерации данных.
    /// </summary>
    [Binding]
    public class GeneratorSteps
    {
        private readonly VariableController variableController;
        private readonly IGenerator generator;

        /// <summary>
        /// Привязка общих шагов к работе с переменным через контекст.
        /// Инициализация генератора.
        /// </summary>
        /// <param name="variableController">Контекст для работы с переменными.</param>
        public GeneratorSteps(VariableController variableController, IGenerator generator)
        {
            this.variableController = variableController;
            this.generator = generator;
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
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная \"{varName}\" уже существует");
            var dt = generator.GetDate(day, month, year);
            dt.Should().NotBeNull($"Проверьте корректность создания даты день:{day},месяц:{month},год:{year}");
            this.variableController.SetVariable(varName, dt.GetType(), dt);
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
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var dt = generator.GetDate(day, month, year);
            dt.Should().NotBeNull($"Проверьте корректность создания даты день:{day},месяц:{month},год:{year}");

            dt?.ToString(format);
            this.variableController.SetVariable(varName, dt.GetType(), dt);
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
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var dt = generator.GetTime(hours, minutes, seconds);
            dt.Should().NotBeNull($"Проверьте корректность создания время часы:{hours},минуты:{minutes},секунды:{seconds}");
            this.variableController.SetVariable(varName, dt.GetType(), dt);
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
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var dt = generator.GetTime(hours, minutes, seconds);
            dt.Should().NotBeNull($"Проверьте корректность создания время часы:{hours},минуты:{minutes},секунды:{seconds}");

            dt?.ToString(format);
            this.variableController.SetVariable(varName, dt.GetType(), dt);
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
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var dt = generator.GetTime(hours, minutes, seconds, milliseconds);
            dt.Should().NotBeNull($"Проверьте корректность создания время часы:{hours},минуты:{minutes},секунды:{seconds},милисекунды:{milliseconds}");
            this.variableController.SetVariable(varName, dt.GetType(), dt);
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
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var dt = generator.GetTime(hours, minutes, seconds, milliseconds);
            dt.Should().NotBeNull($"Проверьте корректность создания время часы:{hours},минуты:{minutes},секунды:{seconds},милисекунды:{milliseconds}");

            dt?.ToString(format);
            this.variableController.SetVariable(varName, dt.GetType(), dt);
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
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var dt = generator.GetDateTime(day, month, year, hours, minutes, seconds);
            dt.Should().NotBeNull($"Проверьте корректность создания даты день:{day},месяц:{month},год:{year} и время часы:{hours},минуты:{minutes},секунды:{seconds}");
            this.variableController.SetVariable(varName, dt.GetType(), dt);
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
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var dt = generator.GetDateTime(day, month, year, hours, minutes, seconds);
            dt.Should().NotBeNull($"Проверьте корректность создания даты день:{day},месяц:{month},год:{year} и время часы:{hours},минуты:{minutes},секунды:{seconds}");

            dt?.ToString(format);
            this.variableController.SetVariable(varName, dt.GetType(), dt);
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
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var dt = generator.GetDateTime(day, month, year, hours, minutes, seconds, milliseconds);
            dt.Should().NotBeNull($"Проверьте корректность создания даты день:{day},месяц:{month},год:{year} и время часы:{hours},минуты:{minutes},секунды:{seconds},милисекунды:{milliseconds}");
            this.variableController.SetVariable(varName, dt.GetType(), dt);
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
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var dt = generator.GetDateTime(day, month, year, hours, minutes, seconds, milliseconds);
            dt.Should().NotBeNull($"Проверьте корректность создания даты день:{day},месяц:{month},год:{year} и время часы:{hours},минуты:{minutes},секунды:{seconds},милисекунды:{milliseconds}");

            dt?.ToString(format);
            this.variableController.SetVariable(varName, dt.GetType(), dt);
        }

        /// <summary>
        /// Шаг для сохранения текущей даты в переменную.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю текущую дату в переменную ""(.+)""")]
        public void StoreAsVariableCurrentDate(string varName)
        {
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var now = generator.GetCurrentDateTime();
            this.variableController.SetVariable(varName, now.GetType(), now);
        }

        /// <summary>
        /// Шаг для сохранения текущей даты в переменную, используя конкретный формат.
        /// </summary>
        /// <param name="format">Формат представления даты.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю текущую дату в формате ""(.+)"" в переменную ""(.+)""")]
        public void StoreAsVariableCurrentDateWithFormat(string format, string varName)
        {
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var now = generator.GetCurrentDateTime().ToString(format);
            this.variableController.SetVariable(varName, now.GetType(), now);
        }

        /// <summary>
        /// Шаг для сохранения даты, которая отличается от текущей на определенный срок в переменную.
        /// </summary>
        /// <param name="year">Количество лет от текущей даты.</param>
        /// <param name="month">Количество месяцев от текущей даты.</param>
        /// <param name="day">Количество дней от текущей даты.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю дату, которая отличается от текущей на ""([0-9]+)"" (?:лет|год[а]?) ""([0-9]+)"" (?:месяц|месяц(?:а|ев)) ""([0-9]+)"" (?:день|дн(?:я|ей)) в переменную ""(.+)""")]
        public void StoreAsVariableFutureDateTimeWithDifference(int year, int month, int day, string varName)
        {
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var dt = generator.GetOtherDate(day, month, year);
            dt.Should().NotBeNull();
            this.variableController.SetVariable(varName, dt.GetType(), dt);
        }

        /// <summary>
        /// Шаг для сохранения даты, которая отличается от даты на определенный срок в переменную.
        /// </summary>
        /// <param name="fYear">Количество лет изначальной даты.</param>
        /// <param name="fMonth">Количество месяцев изначальной даты.</param>
        /// <param name="fDay">Количество дней изначальной даты.</param>
        /// <param name="year">Количество лет от даты.</param>
        /// <param name="month">Количество месяцев от даты.</param>
        /// <param name="day">Количество дней от даты.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю дату, которая отличается от ([0-9]{1,2})\.([0-9]{2})\.([0-9]+) на ""([0-9]+)"" (?:лет|год[а]?) ""([0-9]+)"" (?:месяц|месяц(?:а|ев)) ""([0-9]+)"" (?:день|дн(?:я|ей)) в формате ""(.+)"" в переменную ""(.+)""")]
        public void StoreAsVariableFutureDateTime(int fYear, int fMonth, int fDay, int year, int month, int day, string varName)
        {
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var dt = generator.GetDate(fDay, fMonth, fYear);
            dt.Should().NotBeNull($"Проверьте корректность создания даты день:{fDay},месяц:{fMonth},год:{fYear}");

            var fdt = generator.GetOtherDate(day, month, year, dt);
            fdt.Should().NotBeNull();
            this.variableController.SetVariable(varName, fdt.GetType(), fdt);
        }


        /// <summary>
        /// Шаг для сохранения даты, которая отличается от текущей на определенный срок в переменную, используя конкретный формат.
        /// </summary>
        /// <param name="year">Количество лет от текущей даты.</param>
        /// <param name="month">Количество месяцев от текущей даты.</param>
        /// <param name="day">Количество дней от текущей даты.</param>
        /// <param name="format">Формат представления даты.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю дату, которая отличается от текущей на ""([0-9]+)"" (?:лет|год[а]?) ""([0-9]+)"" (?:месяц|месяц(?:а|ев)) ""([0-9]+)"" (?:день|дн(?:я|ей)) в формате ""(.+)"" в переменную ""(.+)""")]
        public void StoreAsVariablePastDateTimeWithDifference(int year, int month, int day, string format, string varName)
        {
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var dt = generator.GetOtherDate(day, month, year);
            dt.Should().NotBeNull();
            dt?.ToString(format);
            this.variableController.SetVariable(varName, dt.GetType(), dt);
        }

        /// <summary>
        /// Шаг для сохранения прошедшей даты, которая отличается от текущей на определенный срок в переменную, используя конкретный формат.
        /// </summary>
        /// <param name="fYear">Количество лет изначальной даты.</param>
        /// <param name="fMonth">Количество месяцев изначальной даты.</param>
        /// <param name="fDay">Количество дней изначальной даты.</param>
        /// <param name="year">Количество лет от текущей даты.</param>
        /// <param name="month">Количество месяцев от текущей даты.</param>
        /// <param name="day">Количество дней от текущей даты.</param>
        /// <param name="format">Формат представления даты.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю дату, которая отличается от ([0-9]{1,2})\.([0-9]{2})\.([0-9]+) на ""([0-9]+)"" (?:лет|год[а]?) ""([0-9]+)"" (?:месяц|месяц(?:а|ев)) ""([0-9]+)"" (?:день|дн(?:я|ей)) в формате ""(.+)"" в переменную ""(.+)""")]
        public void StoreAsVariablePastDateTime(int fYear, int fMonth, int fDay, int year, int month, int day, string format, string varName)
        {
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var dt = generator.GetDate(fDay, fMonth, fYear);
            dt.Should().NotBeNull($"Проверьте корректность создания даты день:{fDay},месяц:{fMonth},год:{fYear}");

            var fdt = generator.GetOtherDate(day, month, year, dt);
            fdt.Should().NotBeNull();
            fdt?.ToString(format);
            this.variableController.SetVariable(varName, fdt.GetType(), fdt);
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
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var str = generator.GetRandomString(len, prefix);
            this.variableController.SetVariable(varName, str.GetType(), str);
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
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var str = generator.GetRandomChars(len, prefix);
            this.variableController.SetVariable(varName, str.GetType(), str);
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
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var str = generator.GetRandomNumbers(len, prefix);
            this.variableController.SetVariable(varName, str.GetType(), str);
        }

        /// <summary>
        /// Шаг для добавления случайного набора букв и цифр в переменную.
        /// </summary>
        /// <param name="len">Длина строки.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю случайный набор букв и цифр длиной ([0-9]+) знаков в переменную ""(.+)""")]
        public void StoreAsVariableRandomString(int len, string varName)
        {
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var str = generator.GetRandomString(len, string.Empty);
            this.variableController.SetVariable(varName, str.GetType(), str);
        }

        /// <summary>
        /// Шаг для добавления случайного набора букв в переменную.
        /// </summary>
        /// <param name="len">Длина строки.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю случайный набор букв длиной ([0-9]+) знаков в переменную ""(.+)""")]
        public void StoreAsVariableRandomChar(int len, string varName)
        {
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var str = generator.GetRandomChars(len, string.Empty);
            this.variableController.SetVariable(varName, str.GetType(), str);
        }

        /// <summary>
        /// Шаг для добавления случайного набора цифр в переменную.
        /// </summary>
        /// <param name="len">Длина строки.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю случайный набор цифр длиной ([0-9]+) знаков в переменную ""(.+)""")]
        public void StoreAsVariableRandomNumber(int len, string varName)
        {
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var str = generator.GetRandomNumbers(len, string.Empty);
            this.variableController.SetVariable(varName, str.GetType(), str);
        }

        /// <summary>
        /// Шаг для сохранения случайного номера телефона в переменную, используя конкретный формат.
        /// Пример формата: (###)###-####.
        /// </summary>
        /// <param name="mask">Маска для телефона.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю случайный номер телефона в формате ""(.+)"" в переменную ""(.+)""")]
        public void StoreAsVariableRandomPhone(string mask, string varName)
        {
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var str = generator.GetRandomPhone(mask);
            this.variableController.SetVariable(varName, str.GetType(), str);
        }

        /// <summary>
        /// Шаг для сохранения UUID в переменную.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю новый (универсальный уникальный идентификатор|UUID) в переменную ""(.+)""")]
        public void StoreAsVariableUuid(string varName)
        {
            this.variableController.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var str = generator.GetGuid();
            this.variableController.SetVariable(varName, str.GetType(), str);
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
            var networkCredential = new NetworkCredential(username, EvidentInstruction.Helpers.Encryptor.Decrypt(password), domain);
            credentialCache.Add(new Uri(host), authType.ToString(), networkCredential);
            this.variableController.SetVariable(varName, credentialCache.GetType(), credentialCache);
        }

        /// <summary>
        /// Шаг для преобразования значения одной переменной в массив.
        /// </summary>
        /// <param name="varName">Исходная переменная.</param>
        /// <param name="chars">Массив символов-разделителей.</param>
        /// <param name="newVarName">Переменная-результат.</param>
        [StepDefinition(@"я преобразую значение переменной ""(.+)"" в массив, используя символы ""(.+)"" и сохраняю в переменную ""(.+)""")]
        public void StoreVariableValueToArrayVariable(string varName, string chars, string newVarName)
        {
            this.variableController.Variables.ContainsKey(this.variableController.GetVariableName(varName)).Should().BeTrue($"Переменной '{varName}' не существует");
            this.variableController.Variables.ContainsKey(newVarName).Should().BeFalse($"Переменная '{newVarName}' уже существует");

            var str = this.variableController.GetVariableValueText(varName);
            str.Should().NotBeNull($"Значения в переменной {varName} нет");

            var enumerable = EvidentInstruction.Helpers.Converter.CreateEnumerable(str, chars);

            this.variableController.SetVariable(newVarName, enumerable.GetType(), enumerable);
        }
    }
}