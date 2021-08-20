using Microsoft.Extensions.Logging;
using Molder.Controllers;
using Molder.Extensions;
using Molder.Generator.Extensions;
using Molder.Helpers;
using Molder.Models.ReplaceMethod;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Xunit;

namespace Molder.Generator.Tests
{
    [ExcludeFromCodeCoverage]
    public class ReplaceTests
    {
        private readonly VariableController variableContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaceTests"/> class.
        /// Инициализация переменных.
        /// </summary>
        public ReplaceTests()
        {
            var obj = ReplaceMethods.Get() as List<Type>;
            if (!obj.Contains(typeof(GenerationFunctions)))
            {
                (ReplaceMethods.Get() as List<Type>).Add(typeof(GenerationFunctions));
            }

            variableContext = new VariableController();
        }

        /// <summary>
        /// Проверка замены функции для генерации текущей даты.
        /// </summary>
        [Fact]
        public void ReplaceVariables_currentDateTime_ReturnReplaced()
        {
            const string str = "{{currentDateTime(dd-MM-yyyy)}}";
            var outStr = variableContext.ReplaceVariables(str);
            Log.Logger().LogWarning($"ReplaceVariables_currentDateTime_ReturnReplaced - {outStr}");
            Assert.True(DateTime.TryParseExact(outStr, "dd-MM-yyyy",
                              new CultureInfo("en-US"),
                              DateTimeStyles.None,
                              out _));
        }

        /// <summary>
        /// Проверка замены функции для генерации текущей даты.
        /// </summary>
        [Fact]
        public void ReplaceVariables_currentDateTimeWithEmptyLength_ReturnReplaced()
        {
            const string str = "{{currentDateTime()}}";
            var outStr = variableContext.ReplaceVariables(str);
            Assert.Equal("currentDateTime()", outStr);
        }

        /// <summary>
        /// Проверка замены функции для генерации будущей даты.
        /// </summary>
        [Fact]
        public void ReplaceVariables_futureDateTime_ReturnReplaced()
        {
            const string str = "{{futureDateTime(dd-MM-yyyy)}}";
            var outStr = variableContext.ReplaceVariables(str);
            Log.Logger().LogWarning($"ReplaceVariables_futureDateTime_ReturnReplaced - {outStr}");

            Assert.True(DateTime.TryParseExact(outStr, "dd-MM-yyyy",
                              new CultureInfo("en-US"),
                              DateTimeStyles.None,
                              out _));
        }

        /// <summary>
        /// Проверка замены функции для генерации будущей даты.
        /// </summary>
        [Fact]
        public void ReplaceVariables_futureDateTimeWithEmptyLength_ReturnReplaced()
        {
            const string str = "{{futureDateTime()}}";
            var outStr = variableContext.ReplaceVariables(str);
            Assert.Equal("futureDateTime()", outStr);
        }

        /// <summary>
        /// Проверка замены функции для генерации прошлой даты.
        /// </summary>
        [Fact]
        public void ReplaceVariables_pastDateTime_ReturnReplaced()
        {
            const string str = "{{pastDateTime(dd-MM-yyyy)}}";
            var outStr = variableContext.ReplaceVariables(str);
            Log.Logger().LogWarning($"ReplaceVariables_pastDateTime_ReturnReplaced - {outStr}");

            Assert.True(DateTime.TryParseExact(outStr, "dd-MM-yyyy",
                              new CultureInfo("en-US"),
                              DateTimeStyles.None,
                              out _));
        }

        /// <summary>
        /// Проверка замены функции для генерации прошлой даты.
        /// </summary>
        [Fact]
        public void ReplaceVariables_pastDateTimeWithEmptyLength_ReturnReplaced()
        {
            const string str = "{{pastDateTime()}}";
            var outStr = variableContext.ReplaceVariables(str);
            Assert.Equal("pastDateTime()", outStr);
        }

        /// <summary>
        /// Проверка замены функции для генерации рандомной даты.
        /// </summary>
        [Fact]
        public void ReplaceVariables_randomDateTime_ReturnReplaced()
        {
            const string str = "{{randomDateTime(dd-MM-yyyy)}}";
            var outStr = variableContext.ReplaceVariables(str);
            Log.Logger().LogWarning($"ReplaceVariables_randomDateTime_ReturnReplaced - {outStr}");

            Assert.True(DateTime.TryParseExact(outStr, "dd-MM-yyyy",
                              new CultureInfo("en-US"),
                              DateTimeStyles.None,
                              out _));
        }

        /// <summary>
        /// Проверка замены функции для генерации рандомной даты.
        /// </summary>
        [Fact]
        public void ReplaceVariables_randomDateTimeWithEmptyLength_ReturnReplaced()
        {
            const string str = "{{randomDateTime()}}";
            var outStr = variableContext.ReplaceVariables(str);
            Assert.Equal("randomDateTime()", outStr);
        }

        /// <summary>
        /// Проверка замены функции для генерации int числа.
        /// </summary>
        [Fact]
        public void ReplaceVariables_RandomInt_ReturnReplaced()
        {
            const string str = "{{randomInt(4)}}";
            var outStr = variableContext.ReplaceVariables(str);
            Assert.True(int.TryParse(outStr, out _));
        }

        /// <summary>
        /// Проверка замены функции для генерации int числа.
        /// </summary>
        [Fact]
        public void ReplaceVariables_RandomIntWithEmptyLength_ReturnReplaced()
        {
            const string str = "{{randomInt()}}";
            var outStr = variableContext.ReplaceVariables(str);
            Assert.Equal("randomInt()", outStr);
        }

        /// <summary>
        /// Проверка замены функции для генерации рандомных символов.
        /// </summary>
        [Fact]
        public void ReplaceVariables_randomChars_ReturnReplaced()
        {
            const string str = "{{randomChars(20)}}";
            var outStr = variableContext.ReplaceVariables(str);
            Assert.True(outStr.Length == 20);
        }

        /// <summary>
        /// Проверка замены функции для генерации рандомных символов.
        /// </summary>
        [Fact]
        public void ReplaceVariables_randomCharsWithEmptyLength_ReturnReplaced()
        {
            const string str = "{{randomChars()}}";
            var outStr = variableContext.ReplaceVariables(str);
            Assert.Equal("randomChars()", outStr);
        }

        /// <summary>
        /// Проверка замены функции для генерации рандомных символов.
        /// </summary>
        [Fact]
        public void ReplaceVariables_randomString_ReturnReplaced()
        {
            const string str = "{{randomString(20)}}";
            var outStr = variableContext.ReplaceVariables(str);
            Assert.True(outStr.Length == 20);
        }

        /// <summary>
        /// Проверка замены функции для генерации рандомных символов.
        /// </summary>
        [Fact]
        public void ReplaceVariables_randomStringWithEmptyLength_ReturnReplaced()
        {
            const string str = "{{randomString()}}";
            var outStr = variableContext.ReplaceVariables(str);
            Assert.Equal("randomString()", outStr);
        }

        /// <summary>
        /// Проверка замены функции для генерации рандомных символов.
        /// </summary>
        [Fact]
        public void ReplaceVariables_randomPhone_ReturnReplaced()
        {
            const string str = "{{randomPhone(+7##########)}}";
            var outStr = variableContext.ReplaceVariables(str);
            Assert.True(outStr.Length == 12);
        }

        /// <summary>
        /// Проверка замены функции для генерации рандомных символов.
        /// </summary>
        [Fact]
        public void ReplaceVariables_randomPhoneWithEmptyLength_ReturnReplaced()
        {
            const string str = "{{randomPhone()}}";
            var outStr = variableContext.ReplaceVariables(str);
            Assert.Equal("randomPhone()", outStr);
        }
    }
}