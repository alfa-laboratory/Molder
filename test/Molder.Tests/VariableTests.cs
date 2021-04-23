using Molder.Controllers;
using Molder.Extensions;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Molder.Tests
{
    /// <summary>
    /// Тесты проверки работы с переменными.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class VariableTests
    {
        private readonly VariableController variableContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableTests"/> class.
        /// Инициализация переменных.
        /// </summary>
        public VariableTests()
        {
            this.variableContext = new VariableController();
            this.variableContext.SetVariable("first", typeof(string), "1");
            this.variableContext.SetVariable("second", typeof(string), "2");
            this.variableContext.SetVariable("third", typeof(string), "3");
            var token = JToken.Parse("{\"first\":1,\"list\":[{\"second\":2},{\"third\":3}],\"four\":\"4\"}");
            this.variableContext.SetVariable("token", token.GetType(), token);
        }

        /// <summary>
        /// Check Replace XML to Valid XML.
        /// </summary>
        /// <param name="xml">Входная xml.</param>
        /// <param name="validXml">Корректная xml.</param>
        [Theory]
        [InlineData(
            @"<inParms><code>{{first}}</code><pin>{{second}}</pin><list><recordSetRow><code>{{third}}</code></recordSetRow></list></inParms>",
            @"<inParms><code>1</code><pin>2</pin><list><recordSetRow><code>3</code></recordSetRow></list></inParms>")]
        [InlineData(
            @"<inParms><code>{{first}}</code><pin>{{second}}</pin></inParms>",
            @"<inParms><code>1</code><pin>2</pin></inParms>")]
        [InlineData(
            @"<inParms><code>{{token.//first}}</code><pin>{{token.//list[0].second}}</pin></inParms>",
            @"<inParms><code>1</code><pin>2</pin></inParms>")]
        [InlineData(
            @"<code>{{first}}</code>",
            @"<code>1</code>")]
        public void ReplaceVariables_CorrectXML_ReturnReplacedXml(string xml, string validXml)
        {
            var outXml = this.variableContext.ReplaceVariables(xml);

            Assert.Equal(outXml, validXml);
        }

        /// <summary>
        /// Проверка замены переменных в json.
        /// </summary>
        /// <param name="json">Входная json.</param>
        /// <param name="validJson">Корректный json объект.</param>
        [Theory]
        [InlineData(
            "{\"code\":{{first}},\"list\":[{\"A\":{{second}}},{\"B\":{{third}}}],\"block\":\"{{first}}\"}",
            "{\"code\":1,\"list\":[{\"A\":2},{\"B\":3}],\"block\":\"1\"}")]
        [InlineData(
            "{\"list\":[{\"A\":{{second}}},{\"B\":{{third}}}]\"}",
            "{\"list\":[{\"A\":2},{\"B\":3}]\"}")]
        [InlineData(
            "{\"code\":{{first}},\"block\":\"{{first}}\"}",
            "{\"code\":1,\"block\":\"1\"}")]
        public void ReplaceVariables_CorrectJson_ReturnReplacedJson(string json, string validJson)
        {
            var outJson = this.variableContext.ReplaceVariables(json);

            Assert.Equal(outJson, validJson);
        }

        /// <summary>
        /// Проверка замены переменных в json при отсутствии переменной.
        /// </summary>
        /// <param name="json">Входная json.</param>
        /// <param name="validJson">Корректная json.</param>
        [Theory]
        [InlineData(
            "{\"code\":{{four}}\"}",
            "{\"code\":four\"}")]
        public void ReplaceVariables_JsonWithEmptyVariable_ReturnReplacedJson(string json, string validJson)
        {
            var outJson = this.variableContext.ReplaceVariables(json);
            Assert.Equal(outJson, validJson);
        }

        /// <summary>
        /// Проверка замены переменных в xml при пустых скобках.
        /// </summary>
        /// <param name="xml">Входная xml.</param>
        /// <param name="validXML">Корректная xml.</param>
        [Theory]
        [InlineData(
            @"<code>{{}}</code>",
            @"<code></code>")]
        public void ReplaceVariables_EmptyXMLVariable_ReturnReplacedEmpty(string xml, string validXML)
        {
            var outXml = this.variableContext.ReplaceVariables(xml);

            Assert.Equal(outXml, validXML);
        }

        /// <summary>
        /// Проверка замены переменных в xml без переменных во входной xml.
        /// </summary>
        /// <param name="xml">Входная xml.</param>
        /// <param name="validXML">Корректная xml.</param>
        [Theory]
        [InlineData(
            @"<code>1</code>",
            @"<code>1</code>")]
        public void ReplaceVariables_DontReplaceXML_ReturnInXml(string xml, string validXML)
        {
            var outXml = this.variableContext.ReplaceVariables(xml);

            Assert.Equal(outXml, validXML);
        }

        [Theory]
        [InlineData(
            "{\"code\":{{token.//first}},\"list\":[{\"A\":{{token.//list[0].second}}},{\"B\":{{token.//list[1].third}}}],\"block\":\"{{token.//four}}\"}",
            "{\"code\":1,\"list\":[{\"A\":2},{\"B\":3}],\"block\":\"4\"}")]
        [InlineData(
            "{\"list\":[{\"A\":{{token.//list[0].second}}},{\"B\":{{token.//list[1].third}}}]\"}",
            "{\"list\":[{\"A\":2},{\"B\":3}]\"}")]
        [InlineData(
            "{\"code\":{{token.//first}},\"block\":\"{{token.//four}}\"}",
            "{\"code\":1,\"block\":\"4\"}")]
        public void ReplaceVariables_VariableJson_ReturnReplaced(string json, string expected)
        {
            var actual = this.variableContext.ReplaceVariables(json);
            Assert.Equal(expected, actual);
        }
    }
}
