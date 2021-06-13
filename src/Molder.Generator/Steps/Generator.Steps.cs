using System;
using System.Net;
using FluentAssertions;
using Molder.Controllers;
using Molder.Helpers;
using Molder.Infrastructure;
using TechTalk.SpecFlow;
using Molder.Generator.Extensions;
using Molder.Generator.Models.Generators;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Molder.Extensions;
using Microsoft.Extensions.Logging;

namespace Molder.Generator.Steps
{
    /// <summary>
    /// Общие шаги для генерации данных.
    /// </summary>
    [Binding]
    public class GeneratorSteps 
    {
        private string _locale = string.Empty;
        public IFakerGenerator fakerGenerator = null;

        private readonly VariableController variableController;
        private readonly FeatureContext featureContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="Generator"/> class.
        /// Привязка общих шагов к работе с переменным через контекст.
        /// </summary>
        /// <param name="variableController">Контекст для работы с переменными.</param>
        public GeneratorSteps(VariableController variableController, FeatureContext featureContext)
        {
            this.variableController = variableController;
            this.featureContext = featureContext;
            fakerGenerator = new FakerGenerator();
        }

        [ExcludeFromCodeCoverage]
        [BeforeScenario(Order = -20000)]
        public void BeforeScenario()
        {
            _locale = this.featureContext.Locale();
            fakerGenerator = new FakerGenerator
            {
                Locale = _locale
            };
            ((FakerGenerator)fakerGenerator).ReloadLocale();
        }

        #region Store DateTime
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
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
            var dt = fakerGenerator.GetDate(day, month, year);
            dt.Should().NotBeNull($"проверьте корректность создания даты day:{day},month:{month},year:{year}");
            Log.Logger().LogInformation($"Result date is equal to {Environment.NewLine}{dt}");
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
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
            var dt = fakerGenerator.GetDate(day, month, year);

            dt.Should().NotBeNull($"проверьте корректность создания даты day:{day},month:{month},year:{year}");
            var strDate = dt?.ToString(format);

            Log.Logger().LogInformation($"Result date is equal to {Environment.NewLine}{strDate}");
            this.variableController.SetVariable(varName, strDate.GetType(), strDate);
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
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
            var dt = fakerGenerator.GetDateTime(1, 1, 1, hours, minutes, seconds, milliseconds);
            dt.Should().NotBeNull($"проверьте корректность создания времени hours:{hours},minutes:{minutes},seconds:{seconds},milliseconds:{milliseconds}");

            Log.Logger().LogInformation($"Result time is equal to {Environment.NewLine}{dt}");
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
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

            var dt = fakerGenerator.GetDateTime(1, 1, 1, hours, minutes, seconds, milliseconds);
            dt.Should().NotBeNull($"проверьте корректность создания времени hours:{hours},minutes:{minutes},seconds:{seconds},milliseconds:{milliseconds}");
            var time = dt?.ToString(format);

            Log.Logger().LogInformation($"Result time is equal to {Environment.NewLine}{time}");
            this.variableController.SetVariable(varName, time.GetType(), time);
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
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

            var dt = fakerGenerator.GetDateTime(day, month, year, hours, minutes, seconds);
            dt.Should().NotBeNull($"проверьте корректность создания даты и времени day:{day},month:{month},year:{year},hours:{hours},minutes:{minutes},seconds:{seconds}");

            Log.Logger().LogInformation($"Result dateTime is equal to {Environment.NewLine}{dt}");
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
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

            var dt = fakerGenerator.GetDateTime(day, month, year, hours, minutes, seconds);
            dt.Should().NotBeNull($"проверьте корректность создания даты и времени day:{day},month:{month},year:{year},hours:{hours},minutes:{minutes},seconds:{seconds}");

            var dateTime = dt?.ToString(format);

            Log.Logger().LogInformation($"Result dateTime is equal to {Environment.NewLine}{dateTime}");
            this.variableController.SetVariable(varName, dateTime.GetType(), dateTime);
        }

        #endregion
        #region Current DateTime
        /// <summary>
        /// Шаг для сохранения текущей даты в переменную.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю текущую дату в переменную ""(.+)""")]
        public void StoreAsVariableCurrentDate(string varName)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

            var now = fakerGenerator.Current();

            Log.Logger().LogInformation($"Result date is equal to {Environment.NewLine}{now}");
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
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

            var now = fakerGenerator.Current().ToString(format);

            Log.Logger().LogInformation($"Result date is equal to {Environment.NewLine}{now}");
            this.variableController.SetVariable(varName, now.GetType(), now);
        }
        #endregion
        #region Random DateTime
        [StepDefinition(@"я сохраняю рандомную дату в переменную ""(.+)""")]
        public void StoreAsVariableRandomDateTime(string varName)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

            var dt = fakerGenerator.Between();
            Log.Logger().LogInformation($"Result date is equal to {Environment.NewLine}{dt}");
            this.variableController.SetVariable(varName, dt.GetType(), dt);
        }

        [StepDefinition(@"я сохраняю рандомную дату в формате ""(.+)"" в переменную ""(.+)""")]
        public void StoreAsVariableRandomDateTime(string format, string varName)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

            var dt = fakerGenerator.Between();
            var randomDateTime = dt.ToString(format);
            this.variableController.SetVariable(varName, randomDateTime.GetType(), randomDateTime);
        }
        #endregion
        #region Past DateTime

        /// <summary>
        /// Шаг для сохранения прошедшей даты, которая отличается от текущей на определенный срок в переменную.
        /// </summary>
        /// <param name="year">Количество лет от текущей даты.</param>
        /// <param name="month">Количество месяцев от текущей даты.</param>
        /// <param name="day">Количество дней от текущей даты.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю прошедшую дату, которая отличается от текущей на ""([0-9]+)"" (?:лет|год[а]?) ""([0-9]+)"" (?:месяц|месяц(?:а|ев)) ""([0-9]+)"" (?:день|дн(?:я|ей)) в переменную ""(.+)""")]
        public void StoreAsVariablePastDateTimeWithDifference(int year, int month, int day, string varName)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

            var dt = fakerGenerator.GetDate(day, month, year, false);
            dt.Should().NotBeNull($"проверьте корректность создания даты day:{day},month:{month},year:{year}");

            Log.Logger().LogInformation($"Result date is equal to {Environment.NewLine}{dt}");
            this.variableController.SetVariable(varName, dt.GetType(), dt);
        }

        /// <summary>
        /// Шаг для сохранения прошедшей даты, которая отличается от текущей на определенный срок в переменную, используя конкретный формат.
        /// </summary>
        /// <param name="year">Количество лет от текущей даты.</param>
        /// <param name="month">Количество месяцев от текущей даты.</param>
        /// <param name="day">Количество дней от текущей даты.</param>
        /// <param name="format">Формат представления даты.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю прошедшую дату, которая отличается от текущей на ""([0-9]+)"" (?:лет|год[а]?) ""([0-9]+)"" (?:месяц|месяц(?:а|ев)) ""([0-9]+)"" (?:день|дн(?:я|ей)) в формате ""(.+)"" в переменную ""(.+)""")]
        public void StoreAsVariablePastDateTimeWithDifference(int year, int month, int day, string format, string varName)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

            var dt = fakerGenerator.GetDate(day, month, year, false);
            dt.Should().NotBeNull($"проверьте корректность создания даты day:{day},month:{month},year:{year}");
            var pastDateTime = dt?.ToString(format);

            Log.Logger().LogInformation($"Result date is equal to {Environment.NewLine}{pastDateTime}");
            this.variableController.SetVariable(varName, pastDateTime.GetType(), pastDateTime);
        }

        /// <summary>
        /// Шаг для сохранения прошедшей даты, которая отличается от текущей на определенный срок в переменную, используя конкретный формат.
        /// </summary>
        /// <param name="year">Количество лет от текущей даты.</param>
        /// <param name="month">Количество месяцев от текущей даты.</param>
        /// <param name="day">Количество дней от текущей даты.</param>
        /// <param name="format">Формат представления даты.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю прошедшую дату, которая отличается от ([0-9]{1,2})\.([0-9]{2})\.([0-9]+) на ""([0-9]+)"" (?:лет|год[а]?) ""([0-9]+)"" (?:месяц|месяц(?:а|ев)) ""([0-9]+)"" (?:день|дн(?:я|ей)) в переменную ""(.+)""")]
        public void StoreAsVariablePastDateTime(int fYear, int fMonth, int fDay, int year, int month, int day, string varName)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

            var dt = fakerGenerator.GetDate(fDay, fMonth, fYear);
            dt.Should().NotBeNull($"проверьте корректность создания даты day:{fDay},month:{fMonth},year:{fYear}");

            var pdt = fakerGenerator.GetDate(day, month, year, false, dt);
            pdt.Should().NotBeNull($"проверьте корректность создания даты day:{day},month:{month},year:{year}");

            Log.Logger().LogInformation($"Result date is equal to {Environment.NewLine}{pdt}");
            this.variableController.SetVariable(varName, pdt.GetType(), pdt);
        }

        /// <summary>
        /// Шаг для сохранения прошедшей даты, которая отличается от текущей на определенный срок в переменную, используя конкретный формат.
        /// </summary>
        /// <param name="year">Количество лет от текущей даты.</param>
        /// <param name="month">Количество месяцев от текущей даты.</param>
        /// <param name="day">Количество дней от текущей даты.</param>
        /// <param name="format">Формат представления даты.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю прошедшую дату, которая отличается от ([0-9]{1,2})\.([0-9]{2})\.([0-9]+) на ""([0-9]+)"" (?:лет|год[а]?) ""([0-9]+)"" (?:месяц|месяц(?:а|ев)) ""([0-9]+)"" (?:день|дн(?:я|ей)) в формате ""(.+)"" в переменную ""(.+)""")]
        public void StoreAsVariablePastDateTime(int fYear, int fMonth, int fDay, int year, int month, int day, string format, string varName)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

            var dt = fakerGenerator.GetDate(fDay, fMonth, fYear);
            dt.Should().NotBeNull($"проверьте корректность создания даты day:{fDay},month:{fMonth},year:{fYear}");

            var pdt = fakerGenerator.GetDate(day, month, year, false, dt);
            pdt.Should().NotBeNull($"проверьте корректность создания даты day:{day},month:{month},year:{year}");
            var pastDateTime = pdt?.ToString(format);

            Log.Logger().LogInformation($"Result date is equal to {Environment.NewLine}{pastDateTime}");
            this.variableController.SetVariable(varName, pastDateTime.GetType(), pastDateTime);
        }
        #endregion
        #region Future DateTime
        /// <summary>
        /// Шаг для сохранения будущей даты, которая отличается от текущей на определенный срок в переменную.
        /// </summary>
        /// <param name="year">Количество лет от текущей даты.</param>
        /// <param name="month">Количество месяцев от текущей даты.</param>
        /// <param name="day">Количество дней от текущей даты.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю будущую дату, которая отличается от текущей на ""([0-9]+)"" (?:лет|год[а]?) ""([0-9]+)"" (?:месяц|месяц(?:а|ев)) ""([0-9]+)"" (?:день|дн(?:я|ей)) в переменную ""(.+)""")]
        public void StoreAsVariableFutureDateTimeWithDifference(int year, int month, int day, string varName)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

            var dt = fakerGenerator.GetDate(day, month, year, true);
            dt.Should().NotBeNull($"Проверьте корректность создания даты day:{day},month:{month},year:{year}.");

            Log.Logger().LogInformation($"Result date is equal to {Environment.NewLine}{dt}");
            this.variableController.SetVariable(varName, dt.GetType(), dt);
        }


        /// <summary>
        /// Шаг для сохранения будущей даты, которая отличается от текущей на определенный срок в переменную, используя конкретный формат.
        /// </summary>
        /// <param name="year">Количество лет от текущей даты.</param>
        /// <param name="month">Количество месяцев от текущей даты.</param>
        /// <param name="day">Количество дней от текущей даты.</param>
        /// <param name="format">Формат представления даты.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю будущую дату, которая отличается от текущей на ""([0-9]+)"" (?:лет|год[а]?) ""([0-9]+)"" (?:месяц|месяц(?:а|ев)) ""([0-9]+)"" (?:день|дн(?:я|ей)) в формате ""(.+)"" в переменную ""(.+)""")]
        public void StoreAsVariableFutureDateTimeWithDifference(int year, int month, int day, string format, string varName)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

            var dt = fakerGenerator.GetDate(day, month, year, true);
            dt.Should().NotBeNull($"Проверьте корректность создания даты day:{day},month:{month},year:{year}.");
            var futureDateTime = dt?.ToString(format);

            Log.Logger().LogInformation($"Result date is equal to {Environment.NewLine}{futureDateTime}");
            this.variableController.SetVariable(varName, futureDateTime.GetType(), futureDateTime);
        }

        /// <summary>
        /// Шаг для сохранения будущей даты, которая отличается от даты на определенный срок в переменную.
        /// </summary>
        /// <param name="year">Количество лет от текущей даты.</param>
        /// <param name="month">Количество месяцев от текущей даты.</param>
        /// <param name="day">Количество дней от текущей даты.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю будущую дату, которая отличается от ([0-9]{1,2})\.([0-9]{2})\.([0-9]+) на ""([0-9]+)"" (?:лет|год[а]?) ""([0-9]+)"" (?:месяц|месяц(?:а|ев)) ""([0-9]+)"" (?:день|дн(?:я|ей)) в переменную ""(.+)""")]
        public void StoreAsVariableFutureDateTime(int fYear, int fMonth, int fDay, int year, int month, int day, string varName)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

            var dt = fakerGenerator.GetDate(fDay, fMonth, fYear);
            dt.Should().NotBeNull($"Проверьте корректность создания даты day:{fDay},month:{fMonth},year:{fYear}.");

            var fdt = fakerGenerator.GetDate(day, month, year, true, dt);
            fdt.Should().NotBeNull($"Проверьте корректность создания даты day:{day},month:{month},year:{year}.");

            Log.Logger().LogInformation($"Result date is equal to {Environment.NewLine}{fdt}");
            this.variableController.SetVariable(varName, fdt.GetType(), fdt);
        }

        /// <summary>
        /// Шаг для сохранения будущей даты, которая отличается от даты на определенный срок в переменную, используя конкретный формат.
        /// </summary>
        /// <param name="year">Количество лет от текущей даты.</param>
        /// <param name="month">Количество месяцев от текущей даты.</param>
        /// <param name="day">Количество дней от текущей даты.</param>
        /// <param name="format">Формат представления даты.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю будущую дату, которая отличается от ([0-9]{1,2})\.([0-9]{2})\.([0-9]+) на ""([0-9]+)"" (?:лет|год[а]?) ""([0-9]+)"" (?:месяц|месяц(?:а|ев)) ""([0-9]+)"" (?:день|дн(?:я|ей)) в формате ""(.+)"" в переменную ""(.+)""")]
        public void StoreAsVariableFutureDateTime(int fYear, int fMonth, int fDay, int year, int month, int day, string format, string varName)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

            var dt = fakerGenerator.GetDate(fDay, fMonth, fYear);
            dt.Should().NotBeNull($"Проверьте корректность создания даты day:{fDay},month:{fMonth},year:{fYear}.");

            var fdt = fakerGenerator.GetDate(day, month, year, true, dt);
            fdt.Should().NotBeNull($"Проверьте корректность создания даты day:{day},month:{month},year:{year}.");
            var futureDateTime = fdt?.ToString(format);

            Log.Logger().LogInformation($"Result date is equal to {Environment.NewLine}{futureDateTime}");
            this.variableController.SetVariable(varName, futureDateTime.GetType(), futureDateTime);
        }
        #endregion
        #region Random string with prefix
        /// <summary>
        /// Шаг для сохранения случанойго набора букв и цифр в переменную, используя конкретный префикс.
        /// </summary>
        /// <param name="len">Длина строки.</param>
        /// <param name="prefix">Префикс.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю случайный набор букв и цифр длиной ([0-9]+) знаков с префиксом ""(.+)"" в переменную ""(.+)""")]
        public void StoreAsVariableRandomStringWithPrefix(int len, string prefix, string varName)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
            len.Check(prefix, string.Empty);

            var str = prefix + fakerGenerator.String(len - prefix.Length);
            Log.Logger().LogInformation($"Result string is equal to {Environment.NewLine}{str}");
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
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
            len.Check(prefix, string.Empty);

            var str = prefix + fakerGenerator.Chars(len - prefix.Length);
            Log.Logger().LogInformation($"Result string is equal to {Environment.NewLine}{str}");
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
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
            len.Check(prefix, string.Empty);

            var str = prefix + fakerGenerator.Numbers(len - prefix.Length);
            Log.Logger().LogInformation($"Result string is equal to {Environment.NewLine}{str}");
            this.variableController.SetVariable(varName, str.GetType(), str);
        }
        #endregion
        #region Random string with postfix
        /// <summary>
        /// Шаг для сохранения случанойго набора букв и цифр в переменную, используя конкретный постфикс.
        /// </summary>
        /// <param name="len">Длина строки.</param>
        /// <param name="postfix">Постфикс.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю случайный набор букв и цифр длиной ([0-9]+) знаков с постфиксом ""(.+)"" в переменную ""(.+)""")]
        public void StoreAsVariableRandomStringWithPostFix(int len, string postfix, string varName)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
            len.Check(string.Empty, postfix);

            var str = fakerGenerator.String(len - postfix.Length) + postfix;
            Log.Logger().LogInformation($"Result string is equal to {Environment.NewLine}{str}");
            this.variableController.SetVariable(varName, str.GetType(), str);
        }

        /// <summary>
        /// Шаг для добавления случайного набора букв в переменную, используя конкретный префикс.
        /// </summary>
        /// <param name="len">Длина строки.</param>
        /// <param name="postfix">Постфикс.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю случайный набор букв длиной ([0-9]+) знаков с постфиксом ""(.+)"" в переменную ""(.+)""")]
        public void StoreAsVariableRandomCharWithPostfix(int len, string postfix, string varName)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
            len.Check(string.Empty, postfix);

            var str = fakerGenerator.Chars(len - postfix.Length) + postfix;
            Log.Logger().LogInformation($"Result string is equal to {Environment.NewLine}{str}");
            this.variableController.SetVariable(varName, str.GetType(), str);
        }

        /// <summary>
        /// Шаг для добавления случайного набора цифр в переменную, ипользуя конкретный префикс.
        /// </summary>
        /// <param name="len">Длина строки.</param>
        /// <param name="postfix">Постфикс.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю случайный набор цифр длиной ([0-9]+) знаков с постфиксом ""(.+)"" в переменную ""(.+)""")]
        public void StoreAsVariableRandomNumberWithPostfix(int len, string postfix, string varName)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
            len.Check(string.Empty, postfix);

            var str = fakerGenerator.Numbers(len - postfix.Length) + postfix;
            Log.Logger().LogInformation($"Result string is equal to {Environment.NewLine}{str}");
            this.variableController.SetVariable(varName, str.GetType(), str);
        }
        #endregion
        #region Random string
        /// <summary>
        /// Шаг для добавления случайного набора букв и цифр в переменную.
        /// </summary>
        /// <param name="len">Длина строки.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю случайный набор букв и цифр длиной ([0-9]+) знаков в переменную ""(.+)""")]
        public void StoreAsVariableRandomString(int len, string varName)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
            len.Check();
            var str = fakerGenerator.String(len);

            Log.Logger().LogInformation($"Result string is equal to {Environment.NewLine}{str}");
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
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
            len.Check();
            var str = fakerGenerator.Chars(len);

            Log.Logger().LogInformation($"Result string is equal to {Environment.NewLine}{str}");
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
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
            len.Check();
            var str = fakerGenerator.Numbers(len);

            Log.Logger().LogInformation($"Result string is equal to {Environment.NewLine}{str}");
            this.variableController.SetVariable(varName, str.GetType(), str);
        }
        #endregion

        /// <summary>
        /// Шаг для сохранения случайного номера телефона в переменную, используя конкретный формат.
        /// Пример формата: 7##########.
        /// </summary>
        /// <param name="mask">Маска для телефона.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю случайный номер телефона в формате ""(.+)"" в переменную ""(.+)""")]
        public void StoreAsVariableRandomPhone(string mask, string varName)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
            var str = fakerGenerator.Phone(mask);
            Log.Logger().LogInformation($"Result string is equal to {Environment.NewLine}{str}");
            this.variableController.SetVariable(varName, str.GetType(), str);
        }

        /// <summary>
        /// Шаг для сохранения UUID в переменную.
        /// </summary>
        /// <param name="varName">Идентификатор переменной.</param>
        [StepDefinition(@"я сохраняю новый (?:универсальный уникальный идентификатор|UUID) в переменную ""(.+)""")]
        public void StoreAsVariableUuid(string varName)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
            var str = fakerGenerator.Guid();
            Log.Logger().LogInformation($"Result string is equal to {Environment.NewLine}{str}");
            this.variableController.SetVariable(varName, str.GetType(), str);
        }

        [StepDefinition(@"я сохраняю случайный месяц в переменную ""(.+)""")]
        public void StoreAsVariableMonth(string varName)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
            var str = fakerGenerator.Month();
            Log.Logger().LogInformation($"Result string is equal to {Environment.NewLine}{str}");
            this.variableController.SetVariable(varName, str.GetType(), str);
        }

        [StepDefinition(@"я сохраняю случайный день недели в переменную ""(.+)""")]
        public void StoreAsVariableWeekday(string varName)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
            var str = fakerGenerator.Weekday();
            Log.Logger().LogInformation($"Result string is equal to {Environment.NewLine}{str}");
            this.variableController.SetVariable(varName, str.GetType(), str);
        }

        [StepDefinition(@"я сохраняю случайный email с провайдером ""(.+)"" в переменную ""(.+)""")]
        public void StoreAsVariableEmail(string provider, string varName)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
            var str = fakerGenerator.Email(provider);
            Log.Logger().LogInformation($"Result string is equal to {Environment.NewLine}{str}");
            this.variableController.SetVariable(varName, str.GetType(), str);
        }

        [StepDefinition(@"я сохраняю случайный Ip адрес в переменную ""(.+)""")]
        public void StoreAsVariableIp(string varName)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
            var str = fakerGenerator.Ip();
            Log.Logger().LogInformation($"Result string is equal to {Environment.NewLine}{str}");
            this.variableController.SetVariable(varName, str.GetType(), str);
        }

        [StepDefinition(@"я сохраняю случайный Url в переменную ""(.+)""")]
        public void StoreAsVariableUrl(string varName)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
            var str = fakerGenerator.Url();
            Log.Logger().LogInformation($"Result string is equal to {Environment.NewLine}{str}");
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
        [ExcludeFromCodeCoverage]
        [StepDefinition(@"я создаю полномочия для хоста ""(.+)"" c типом ""(.+)"" для пользователя с доменом ""(.+)"", логином ""(.+)"", паролем ""(.+)"" и сохраняю в переменную ""(.+)""")]
        public void StoreCredentialsForHostToVariable(string host, AuthType authType, string domain, string username, string password, string varName)
        {
            var _host = this.variableController.ReplaceVariables(host) ?? host;
            var _domain = this.variableController.ReplaceVariables(domain) ?? domain;
            var _username = this.variableController.ReplaceVariables(username) ?? username;
            var _password = this.variableController.ReplaceVariables(password) ?? password;

            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

            var credentialCache = new CredentialCache();
            var networkCredential = new NetworkCredential(_username, _password, _domain);
            credentialCache.Add(new Uri(_host), authType.ToString(), networkCredential);

            Log.Logger().LogInformation($"Create NetworkCredential for {authType.ToString()} with host:{_host}, domain:{_domain}, username:{_username} and password:{_password}.");

            Log.Logger().LogInformation($"Result host credentionals is equal to {Environment.NewLine}{credentialCache}");
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
            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            this.variableController.Variables.Should().NotContainKey(newVarName, $"переменная \"{newVarName}\" уже существует");

            var str = this.variableController.GetVariableValueText(varName);
            str.Should().NotBeNull($"Значения в переменной \"{varName}\" нет");

            var enumerable = Converter.CreateEnumerable(str, chars);
            this.variableController.SetVariable(newVarName, enumerable.GetType(), enumerable);
        }
    }
}