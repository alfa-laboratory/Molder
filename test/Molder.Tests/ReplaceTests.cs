using Molder.Controllers;
using Molder.Extensions;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using Xunit;

namespace Molder.Tests
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
            variableContext = new VariableController();
            variableContext.SetVariable("int", typeof(int), 1);
            variableContext.SetVariable("long", typeof(long), 100);
            variableContext.SetVariable("double", typeof(double), 1.1);
            variableContext.SetVariable("bool", typeof(bool), true);

            var token = JToken.Parse("{\"first\":1,\"list\":[{\"second\":true},{\"third\":3}],\"four\":true}");
            variableContext.SetVariable("json", token.GetType(), token);

            var xml = new XmlDocument();
            xml.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\"?><addresses xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation='test.xsd'><address><name>Joe Tester</name><street>Baker street</street><house>5</house><bool>true</bool></address></addresses>");
            variableContext.SetVariable("xml", xml.GetType(), xml);
        }

        /// <summary>
        /// Проверка замены функции для парсинга int типа.
        /// </summary>
        /// <param name="json">Входная json.</param>
        /// <param name="validJson">Корректная json.</param>
        [Theory]
        [InlineData("{\"code\":{{parseInt(1)}}\"}","{\"code\":1\"}")]
        [InlineData("{\"code\":{{parseInt(int)}}\"}", "{\"code\":1\"}")]
        [InlineData("{\"code\":{{parseInt(json.//first)}}\"}", "{\"code\":1\"}")]
        [InlineData("{\"code\":{{parseInt(xml.//house)}}\"}", "{\"code\":5\"}")]
        public void ReplaceVariables_JsonWithParseInt_ReturnReplacedJson(string json, string validJson)
        {
            var outJson = variableContext.ReplaceVariables(json);
            Assert.Equal(outJson, validJson);
        }

        /// <summary>
        /// Проверка замены функции для парсинга int типа.
        /// </summary>
        /// <param name="json">Входная json.</param>
        /// <param name="validJson">Корректная json.</param>
        [Theory]
        [InlineData("{\"code\":{{parseInt()}}\"}", "{\"code\":parseInt()\"}")]
        public void ReplaceVariables_JsonWithInvalidParseInt_ReturnReplacedJson(string json, string validJson)
        {
            var outJson = variableContext.ReplaceVariables(json);
            Assert.Equal(outJson, validJson);
        }

        /// <summary>
        /// Проверка замены функции для парсинга int типа.
        /// </summary>
        /// <param name="json">Входная json.</param>
        /// <param name="validJson">Корректная json.</param>
        [Theory]
        [InlineData("{\"code\":{{parseInt(null)}}\"}", "{\"code\":null\"}")]
        public void ReplaceVariables_JsonWithInvalidParseIntNull_ReturnReplacedJson(string json, string validJson)
        {
            var outJson = variableContext.ReplaceVariables(json);
            Assert.Equal(outJson, validJson);
        }

        /// <summary>
        /// Проверка замены функции для парсинга long типа.
        /// </summary>
        /// <param name="json">Входная json.</param>
        /// <param name="validJson">Корректная json.</param>
        [Theory]
        [InlineData("{\"code\":{{parseLong(1)}}\"}", "{\"code\":1\"}")]
        [InlineData("{\"code\":{{parseLong(long)}}\"}", "{\"code\":100\"}")]
        [InlineData("{\"code\":{{parseLong(json.//first)}}\"}", "{\"code\":1\"}")]
        [InlineData("{\"code\":{{parseLong(xml.//house)}}\"}", "{\"code\":5\"}")]
        public void ReplaceVariables_JsonWithParseLong_ReturnReplacedJson(string json, string validJson)
        {
            var outJson = variableContext.ReplaceVariables(json);
            Assert.Equal(outJson, validJson);
        }

        /// <summary>
        /// Проверка замены функции для парсинга long типа.
        /// </summary>
        /// <param name="json">Входная json.</param>
        /// <param name="validJson">Корректная json.</param>
        [Theory]
        [InlineData("{\"code\":{{parseLong()}}\"}", "{\"code\":parseLong()\"}")]
        public void ReplaceVariables_JsonWithInvalidParseLong_ReturnReplacedJson(string json, string validJson)
        {
            var outJson = variableContext.ReplaceVariables(json);
            Assert.Equal(outJson, validJson);
        }

        /// <summary>
        /// Проверка замены функции для парсинга long типа.
        /// </summary>
        /// <param name="json">Входная json.</param>
        /// <param name="validJson">Корректная json.</param>
        [Theory]
        [InlineData("{\"code\":{{parseLong(null)}}\"}", "{\"code\":null\"}")]
        public void ReplaceVariables_JsonWithInvalidParseLongNull_ReturnReplacedJson(string json, string validJson)
        {
            var outJson = variableContext.ReplaceVariables(json);
            Assert.Equal(outJson, validJson);
        }

        /// <summary>
        /// Проверка замены функции для парсинга double типа.
        /// </summary>
        /// <param name="json">Входная json.</param>
        /// <param name="validJson">Корректная json.</param>
        [Theory]
        [InlineData("{\"code\":{{parseDouble(1.1)}}\"}", "{\"code\":1.1\"}")]
        //[InlineData("{\"code\":{{parseDouble(double)}}\"}", "{\"code\":1.1\"}")]
        [InlineData("{\"code\":{{parseDouble(json.//first)}}\"}", "{\"code\":1\"}")]
        [InlineData("{\"code\":{{parseDouble(xml.//house)}}\"}", "{\"code\":5\"}")]
        public void ReplaceVariables_JsonWithParseDouble_ReturnReplacedJson(string json, string validJson)
        {
            var outJson = variableContext.ReplaceVariables(json);
            Assert.Equal(outJson, validJson);
        }

        /// <summary>
        /// Проверка замены функции для парсинга double типа.
        /// </summary>
        /// <param name="json">Входная json.</param>
        /// <param name="validJson">Корректная json.</param>
        [Theory]
        [InlineData("{\"code\":{{parseDouble()}}\"}", "{\"code\":parseDouble()\"}")]
        public void ReplaceVariables_JsonWithInvalidParseDouble_ReturnReplacedJson(string json, string validJson)
        {
            var outJson = variableContext.ReplaceVariables(json);
            Assert.Equal(outJson, validJson);
        }

        /// <summary>
        /// Проверка замены функции для парсинга double типа.
        /// </summary>
        /// <param name="json">Входная json.</param>
        /// <param name="validJson">Корректная json.</param>
        [Theory]
        [InlineData("{\"code\":{{parseDouble(null)}}\"}", "{\"code\":null\"}")]
        public void ReplaceVariables_JsonWithInvalidParseDoubleNull_ReturnReplacedJson(string json, string validJson)
        {
            var outJson = variableContext.ReplaceVariables(json);
            Assert.Equal(outJson, validJson);
        }

        /// <summary>
        /// Проверка замены функции для парсинга bool типа.
        /// </summary>
        /// <param name="json">Входная json.</param>
        /// <param name="validJson">Корректная json.</param>
        [Theory]
        [InlineData("{\"code\":{{parseBool(false)}}\"}", "{\"code\":False\"}")]
        [InlineData("{\"code\":{{parseBool(bool)}}\"}", "{\"code\":True\"}")]
        [InlineData("{\"code\":{{parseBool(json.//four)}}\"}", "{\"code\":True\"}")]
        [InlineData("{\"code\":{{parseBool(xml.//bool)}}\"}", "{\"code\":True\"}")]
        public void ReplaceVariables_JsonWithParseBool_ReturnReplacedJson(string json, string validJson)
        {
            var outJson = variableContext.ReplaceVariables(json);
            Assert.Equal(outJson, validJson);
        }

        /// <summary>
        /// Проверка замены функции для парсинга double типа.
        /// </summary>
        /// <param name="json">Входная json.</param>
        /// <param name="validJson">Корректная json.</param>
        [Theory]
        [InlineData("{\"code\":{{parseBool()}}\"}", "{\"code\":parseBool()\"}")]
        public void ReplaceVariables_JsonWithInvalidParseBool_ReturnReplacedJson(string json, string validJson)
        {
            var outJson = variableContext.ReplaceVariables(json);
            Assert.Equal(outJson, validJson);
        }

        /// <summary>
        /// Проверка замены функции для парсинга double типа.
        /// </summary>
        /// <param name="json">Входная json.</param>
        /// <param name="validJson">Корректная json.</param>
        [Theory]
        [InlineData("{\"code\":{{parseBool(null)}}\"}", "{\"code\":null\"}")]
        public void ReplaceVariables_JsonWithInvalidParseBoolNull_ReturnReplacedJson(string json, string validJson)
        {
            var outJson = variableContext.ReplaceVariables(json);
            Assert.Equal(outJson, validJson);
        }
    }
}
