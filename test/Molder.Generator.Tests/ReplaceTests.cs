using Molder.Controllers;
using Molder.Extensions;
using Molder.Generator.Extensions;
using Molder.Models.ReplaceMethod;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

            this.variableContext = new VariableController();
        }

        /// <summary>
        /// Проверка замены функции для генерации текущей даты.
        /// </summary>
        [Fact]
        public void ReplaceVariables_currentDateTime_ReturnReplaced()
        {
            var str = "{{currentDateTime(dd-MM-yyyy)}}";
            var outStr = this.variableContext.ReplaceVariables(str);
            Assert.True(DateTime.TryParse(outStr, out var dateValue));
        }

        /// <summary>
        /// Проверка замены функции для генерации текущей даты.
        /// </summary>
        [Fact]
        public void ReplaceVariables_currentDateTimeWithEmptyLength_ReturnReplaced()
        {
            var str = "{{currentDateTime()}}";
            var outStr = this.variableContext.ReplaceVariables(str);
            Assert.Equal("currentDateTime()", outStr);
        }

        /// <summary>
        /// Проверка замены функции для генерации будущей даты.
        /// </summary>
        [Fact]
        public void ReplaceVariables_futureDateTime_ReturnReplaced()
        {
            var str = "{{futureDateTime(dd-MM-yyyy)}}";
            var outStr = this.variableContext.ReplaceVariables(str);
            Assert.True(DateTime.TryParse(outStr, out var dateValue));
        }

        /// <summary>
        /// Проверка замены функции для генерации будущей даты.
        /// </summary>
        [Fact]
        public void ReplaceVariables_futureDateTimeWithEmptyLength_ReturnReplaced()
        {
            var str = "{{futureDateTime()}}";
            var outStr = this.variableContext.ReplaceVariables(str);
            Assert.Equal("futureDateTime()", outStr);
        }

        /// <summary>
        /// Проверка замены функции для генерации прошлой даты.
        /// </summary>
        [Fact]
        public void ReplaceVariables_pastDateTime_ReturnReplaced()
        {
            var str = "{{pastDateTime(dd-MM-yyyy)}}";
            var outStr = this.variableContext.ReplaceVariables(str);
            Assert.True(DateTime.TryParse(outStr, out var dateValue));
        }

        /// <summary>
        /// Проверка замены функции для генерации прошлой даты.
        /// </summary>
        [Fact]
        public void ReplaceVariables_pastDateTimeWithEmptyLength_ReturnReplaced()
        {
            var str = "{{pastDateTime()}}";
            var outStr = this.variableContext.ReplaceVariables(str);
            Assert.Equal("pastDateTime()", outStr);
        }

        /// <summary>
        /// Проверка замены функции для генерации рандомной даты.
        /// </summary>
        [Fact]
        public void ReplaceVariables_randomDateTime_ReturnReplaced()
        {
            var str = "{{randomDateTime(dd-MM-yyyy)}}";
            var outStr = this.variableContext.ReplaceVariables(str);
            Assert.True(DateTime.TryParse(outStr, out var dateValue));
        }

        /// <summary>
        /// Проверка замены функции для генерации рандомной даты.
        /// </summary>
        [Fact]
        public void ReplaceVariables_randomDateTimeWithEmptyLength_ReturnReplaced()
        {
            var str = "{{randomDateTime()}}";
            var outStr = this.variableContext.ReplaceVariables(str);
            Assert.Equal("randomDateTime()", outStr);
        }

        /// <summary>
        /// Проверка замены функции для генерации int числа.
        /// </summary>
        [Fact]
        public void ReplaceVariables_RandomInt_ReturnReplaced()
        {
            var str = "{{randomInt(4)}}";
            var outStr = this.variableContext.ReplaceVariables(str);
            Assert.True(int.TryParse(outStr, out var value));
        }

        /// <summary>
        /// Проверка замены функции для генерации int числа.
        /// </summary>
        [Fact]
        public void ReplaceVariables_RandomIntWithEmptyLength_ReturnReplaced()
        {
            var str = "{{randomInt()}}";
            var outStr = this.variableContext.ReplaceVariables(str);
            Assert.Equal("randomInt()", outStr);
        }

        /// <summary>
        /// Проверка замены функции для генерации рандомных символов.
        /// </summary>
        [Fact]
        public void ReplaceVariables_randomChars_ReturnReplaced()
        {
            var str = "{{randomChars(20)}}";
            var outStr = this.variableContext.ReplaceVariables(str);
            Assert.True(outStr.Length == 20);
        }

        /// <summary>
        /// Проверка замены функции для генерации рандомных символов.
        /// </summary>
        [Fact]
        public void ReplaceVariables_randomCharsWithEmptyLength_ReturnReplaced()
        {
            var str = "{{randomChars()}}";
            var outStr = this.variableContext.ReplaceVariables(str);
            Assert.Equal("randomChars()", outStr);
        }

        /// <summary>
        /// Проверка замены функции для генерации рандомных символов.
        /// </summary>
        [Fact]
        public void ReplaceVariables_randomString_ReturnReplaced()
        {
            var str = "{{randomString(20)}}";
            var outStr = this.variableContext.ReplaceVariables(str);
            Assert.True(outStr.Length == 20);
        }

        /// <summary>
        /// Проверка замены функции для генерации рандомных символов.
        /// </summary>
        [Fact]
        public void ReplaceVariables_randomStringWithEmptyLength_ReturnReplaced()
        {
            var str = "{{randomString()}}";
            var outStr = this.variableContext.ReplaceVariables(str);
            Assert.Equal("randomString()", outStr);
        }

        /// <summary>
        /// Проверка замены функции для генерации рандомных символов.
        /// </summary>
        [Fact]
        public void ReplaceVariables_randomPhone_ReturnReplaced()
        {
            var str = "{{randomPhone(+7##########)}}";
            var outStr = this.variableContext.ReplaceVariables(str);
            Assert.True(outStr.Length == 12);
        }

        /// <summary>
        /// Проверка замены функции для генерации рандомных символов.
        /// </summary>
        [Fact]
        public void ReplaceVariables_randomPhoneWithEmptyLength_ReturnReplaced()
        {
            var str = "{{randomPhone()}}";
            var outStr = this.variableContext.ReplaceVariables(str);
            Assert.Equal("randomPhone()", outStr);
        }
    }
}
