// <copyright file="UtilsTest.cs" company="AlfaBank">
// Copyright (c) AlfaBank. All rights reserved.
// </copyright>

namespace AlfaBank.AFT.Core.Test.Utils
{
    using System.Diagnostics.CodeAnalysis;
    using AlfaBank.AFT.Core.Model.Context;
    using Xunit;

    /// <summary>
    /// Тесты проверки работы с переменными.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UtilsTest
    {
        private const string XmlPattern = "{([^}]*)}";
        private const string JsonPattern = @"@(.\w+)";
        private readonly VariableContext variableContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UtilsTest"/> class.
        /// Инициализация переменных.
        /// </summary>
        public UtilsTest()
        {
            this.variableContext = new VariableContext();
            this.variableContext.SetVariable("first", typeof(string), "1");
            this.variableContext.SetVariable("second", typeof(string), "2");
            this.variableContext.SetVariable("third", typeof(string), "3");
        }

        /// <summary>
        /// Check Replace XML to Valid XML.
        /// </summary>
        /// <param name="xml">Входная xml.</param>
        /// <param name="validXml">Корректная xml.</param>
        [Theory]
        [InlineData(
            @"<inParms><code>{first}</code><pin>{second}</pin><list><recordSetRow><code>{third}</code></recordSetRow></list></inParms>",
            @"<inParms><code>1</code><pin>2</pin><list><recordSetRow><code>3</code></recordSetRow></list></inParms>")]
        [InlineData(
            @"<inParms><code>{first}</code><pin>{second}</pin></inParms>",
            @"<inParms><code>1</code><pin>2</pin></inParms>")]
        [InlineData(
            @"<code>{first}</code>",
            @"<code>1</code>")]
        public void ReplaceVariables_CorrectXML_ReturnReplacedXml(string xml, string validXml)
        {
            var outXml = this.variableContext.ReplaceVariablesInXmlBody(xml);

            Assert.Equal(outXml, validXml);
        }

        /// <summary>
        /// Проверка замены переменных в json.
        /// </summary>
        /// <param name="json">Входная json.</param>
        /// <param name="validJson">Корректный json объект.</param>
        [Theory]
        [InlineData(
            "{\"code\":@first,\"list\":[{\"A\":@second},{\"B\":@third}],\"block\":\"@first\"}",
            "{\"code\":1,\"list\":[{\"A\":2},{\"B\":3}],\"block\":\"1\"}")]
        [InlineData(
            "{\"list\":[{\"A\":@second},{\"B\":@third}]\"}",
            "{\"list\":[{\"A\":2},{\"B\":3}]\"}")]
        [InlineData(
            "{\"code\":@first,\"block\":\"@first\"}",
            "{\"code\":1,\"block\":\"1\"}")]
        public void ReplaceVariables_CorrectJson_ReturnReplacedJson(string json, string validJson)
        {
            var outJson = this.variableContext.ReplaceVariablesInJsonBody(json);

            Assert.Equal(outJson, validJson);
        }

        /// <summary>
        /// Проверка замены переменных в json при отсутствии переменной.
        /// </summary>
        /// <param name="json">Входная json.</param>
        /// <param name="validJson">Корректная json.</param>
        [Theory]
        [InlineData(
            "{\"code\":@four\"}",
            "{\"code\":four\"}")]
        public void ReplaceVariables_JsonWithEmptyVariable_ReturnReplacedJson(string json, string validJson)
        {
            var outJson = this.variableContext.ReplaceVariablesInJsonBody(json);
            Assert.Equal(outJson, validJson);
        }

        /// <summary>
        /// Проверка замены переменных в xml при пустых скобках.
        /// </summary>
        /// <param name="xml">Входная xml.</param>
        /// <param name="validXML">Корректная xml.</param>
        [Theory]
        [InlineData(
            @"<code>{}</code>",
            @"<code></code>")]
        public void ReplaceVariables_EmptyXMLVariable_ReturnReplacedEmpty(string xml, string validXML)
        {
            var outXml = this.variableContext.ReplaceVariablesInXmlBody(xml);

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
            var outXml = this.variableContext.ReplaceVariablesInXmlBody(xml);

            Assert.Equal(outXml, validXML);
        }
    }
}
