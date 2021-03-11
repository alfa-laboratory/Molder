using FluentAssertions;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using Molder.Controllers;
using Molder.Tests.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Linq;
using Xunit;
using Molder.Models;
using Molder.Infrastructures;

namespace Molder.Tests
{

    [ExcludeFromCodeCoverage]
    public class VariableControllerTests
    {
        private const string XmlPattern = "{([^}]*)}";
        private const string JsonPattern = @"@(.\w+)";
        private readonly VariableController variableContext;

        private const string Xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><addresses xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation='test.xsd'><address><name>Joe Tester</name><street>Baker street</street><house>5</house></address></addresses>";
        private const string Json = "{\"address\": {\"name\": \"Joe Tester\",\"street\": \"Baker street\",\"house\": \"5\"}}";
        /// <summary>
        /// Initializes a new instance of the <see cref="VariableControllerTests"/> class.
        /// Инициализация переменных.
        /// </summary>
        public VariableControllerTests()
        {
            variableContext = new VariableController();
            variableContext.SetVariable("first", typeof(string), "1");
            variableContext.SetVariable("second", typeof(string), "2");
            variableContext.SetVariable("third", typeof(string), "3");
        }

        [Theory]
        [InlineData("first", "first")]
        [InlineData("second", "second")]
        [InlineData("third", "third")]
        public void GetVariableName_CorrectName_ReturnName(string key, string name)
        {
            var varName = variableContext.GetVariableName(key);
            varName.Should().Be(name);
        }
       
        [Theory]
        [InlineData("pointTest.test", "pointTest")]
        [InlineData("bracessTest[test]", "bracessTest")]
        public void GetVariableName_NameWithBracessAndPoint_ReturnName(string key, string name)
        {
            variableContext.SetVariable(key, typeof(string), "1"); 
            var varName = variableContext.GetVariableName(key);                     
            varName.Should().Be(name);            
        }  

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GetVariableName_InCorrectName_ReturnNull(string key)
        { 
            var varName = variableContext.GetVariableName(key);
            varName.Should().BeNull();
        }

        [Theory]
        [InlineData("first", typeof(string) ,"1")]
        [InlineData("second", typeof(string), "2")]
        [InlineData("third", typeof(string), "3")]
        public void GetVariable_CorrectName_ReturnVariable(string key, Type type, string value)
        {
            var variable = variableContext.GetVariable(key);
            variable.Type.Should().Be(type);
            variable.Value.Should().Be(value);
        }

        [Theory]
        [InlineData("zero", typeof(int), 0)]       
        public void GetVariable_TypeofAccessGlobal_ReturnVariable(string key, Type type, int value)
        {
            variableContext.SetVariable(key, type, value, TypeOfAccess.Global);
            var variable = variableContext.GetVariable(key);
            variable.Type.Should().Be(type);
            variable.Value.Should().Be(value);
        }

        [Theory]
        [InlineData("zero", typeof(int), 0)]
        public void GetVariable_TypeofAccessLocal_ReturnVariable(string key, Type type, int value)
        {
            variableContext.SetVariable(key, type, value, TypeOfAccess.Local);
            var variable = variableContext.GetVariable(key);
            variable.Type.Should().Be(type);
            variable.Value.Should().Be(value);
        }

        [Theory]
        [InlineData("zero", typeof(int), 0)]
        public void GetVariable_TypeofAccessDefault_ReturnVariable(string key, Type type, int value)
        {
            variableContext.SetVariable(key, type, value, TypeOfAccess.Default);
            var variable = variableContext.GetVariable(key);
            variable.Type.Should().Be(type);
            variable.Value.Should().Be(value);
        }

        [Theory]
        [InlineData(null, typeof(string), "", TypeOfAccess.Local)]
        [InlineData("", typeof(string), "", TypeOfAccess.Local)]
        public void SetVariable_KeyIsNull_ReturnVariables(string key, Type type, string value, TypeOfAccess typeOfAccess)
        {
            variableContext.SetVariable(key, type, value, typeOfAccess);
            variableContext.Variables.Count.Should().Be(3);
        }

        [Theory]
        [InlineData("zero", typeof(string), "0", TypeOfAccess.Default)]
        public void SetVariable_GlobalAndDefault_ReturnVariables(string key, Type type, string value, TypeOfAccess typeOfAccess)
        {
            variableContext.SetVariable(key, type, key, TypeOfAccess.Global);
            variableContext.SetVariable(key, type, value, typeOfAccess);
            variableContext.Variables[key].TypeOfAccess.Should().Be(TypeOfAccess.Global);
            variableContext.Variables[key].Value.Should().Be(key);
        }

        [Theory]
        [InlineData("zero", typeof(string), "0", TypeOfAccess.Local)]
        public void SetVariable_LocalAndDefault_ReturnVariables(string key, Type type, string value, TypeOfAccess typeOfAccess)
        {
            variableContext.SetVariable(key, type, key, TypeOfAccess.Default);
            variableContext.SetVariable(key, type, value, typeOfAccess);
            variableContext.Variables[key].TypeOfAccess.Should().Be(typeOfAccess);
            variableContext.Variables[key].Value.Should().Be(value);
        }

        [Theory]
        [InlineData("zero", typeof(string), "0", TypeOfAccess.Local)]
        public void SetVariable_GlobalAndLocal_ReturnVariables(string key, Type type, string value, TypeOfAccess typeOfAccess)
        {
            variableContext.SetVariable(key, type, key, TypeOfAccess.Global);
            variableContext.SetVariable(key, type, value, typeOfAccess);
            variableContext.Variables[key].TypeOfAccess.Should().Be(TypeOfAccess.Local);
            variableContext.Variables[key].Value.Should().Be(value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("four")]
        public void GetVariable_InCorrectName_ReturnNull(string key)
        {
            var variable = variableContext.GetVariable(key);
            variable.Should().BeNull();
        }

        [Theory]
        [InlineData("first", true)]
        [InlineData("second", true)]
        [InlineData("four", false)]
        public void CheckVariableByKey_CorrectName_ReturnVariable(string key, bool flag)
        {
            var variable = variableContext.CheckVariableByKey(key);
            variable.Should().Be(flag);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CheckVariableByKey_InCorrectName_ReturnNull(string key)
        {
            var variable = variableContext.CheckVariableByKey(key);
            variable.Should().BeFalse();
        }

        public static IEnumerable<object[]> VariableData()
        {
            yield return new object[] { "int", typeof(int), 4 };
            yield return new object[] { "string", typeof(string), "5" };
            yield return new object[] { "datetime", typeof(DateTime), new DateTime(2000, 01, 01) };
        }

        [Theory]
        [MemberData(nameof(VariableData))]
        public void SetVariable_CorrectVariable_ReturnVariable(string key, Type type, object value)
        {
            variableContext.SetVariable(key, type, value);
            var variable = variableContext.GetVariable(key);
            variable.Type.Should().Be(type);
            variable.Value.Should().Be(value);
        }

        [Theory]
        [InlineData("key", "value", "newValue")]
        public void SetVariable_ContainsVariable_ReturnNewValue(string key, string value, string newValue)
        {
            variableContext.SetVariable(key, typeof(string), value);

            var variable = variableContext.GetVariable(key);
            variable.Type.Should().Be(typeof(string));
            variable.Value.Should().Be(value);

            variableContext.SetVariable(key, typeof(string), newValue);

            variable = variableContext.GetVariable(key);
            variable.Type.Should().Be(typeof(string));
            variable.Value.Should().Be(newValue);
        }

        public static IEnumerable<object[]> VariableValueData()
        {
            yield return new object[] { "xml", typeof(XmlDocument), Xml };
            yield return new object[] { "xDoc", typeof(XDocument), Xml };
            yield return new object[] { "json", typeof(JObject), Json };
            yield return new object[] { "bson", typeof(BsonDocument), Json };
        }

        [Theory]
        [InlineData("xml", typeof(XmlDocument), Xml, "xml", "addresses", "<address><name>Joe Tester</name><street>Baker street</street><house>5</house></address>")]
        [InlineData("xml", typeof(XmlDocument), Xml, "xml.//addresses", "addresses", "<address><name>Joe Tester</name><street>Baker street</street><house>5</house></address>")]
        [InlineData("xml", typeof(XmlDocument), Xml, "xml.//address", "address", "<name>Joe Tester</name><street>Baker street</street><house>5</house>")]
        [InlineData("xml", typeof(XmlDocument), Xml, "xml.//house", "house", "5")]
        public void GetVariableValue_CorrectVariableXml_ReturnValue(string key, Type type, string value, string searchKey, string name, string searchValue)
        {
            var doc = new XmlDocument();
            doc.LoadXml(value);
            variableContext.SetVariable(key, type, doc);

            var variable = variableContext.GetVariableValue(searchKey);

            ((XmlNode)variable).Name.Should().Be(name);
            ((XmlNode)variable).InnerXml.Should().Be(searchValue);
        }

        [Theory]
        [InlineData("xDoc", typeof(XDocument), Xml, "xDoc", "addresses")]
        [InlineData("xDoc", typeof(XDocument), Xml, "xDoc.//addresses", "addresses")]
        [InlineData("xDoc", typeof(XDocument), Xml, "xDoc.//address", "address")]
        [InlineData("xDoc", typeof(XDocument), Xml, "xDoc.//house", "house")]
        public void GetVariableValue_CorrectVariableXDoc_ReturnValue(string key, Type type, string value, string searchKey, string name)
        {
            var doc = XDocument.Parse(value);
            variableContext.SetVariable(key, type, doc);

            var variable = variableContext.GetVariableValue(searchKey);

            ((XElement)variable).Name.LocalName.Should().Be(name);
        }

        [Theory]
        [InlineData("JObject", typeof(JObject), Json, "JObject.//address.street", "Baker street")]
        [InlineData("JObject", typeof(JObject), Json, "JObject.//address.house", "5")]
        public void GetVariableValue_CorrectVariableJObject_ReturnValue(string key, Type type, string value, string searchKey, string searchValue)
        {
            var doc = JObject.Parse(value);
            variableContext.SetVariable(key, type, doc);

            var variable = variableContext.GetVariableValue(searchKey);

            ((JValue)variable).Value.Should().Be(searchValue);
        }

        [Theory]
        [InlineData("JToken", typeof(JToken), Json, "JToken.//address.street", "Baker street")]
        [InlineData("JToken", typeof(JToken), Json, "JToken.//address.house", "5")]
        public void GetVariableValue_CorrectVariableJToken_ReturnValue(string key, Type type, string value, string searchKey, string searchValue)
        {
            var doc = JToken.Parse(value);
            variableContext.SetVariable(key, type, doc);

            var variable = variableContext.GetVariableValue(searchKey);

            ((JValue)variable).Value.Should().Be(searchValue);
        }


        [Theory]
        [InlineData("BsonDocument", typeof(BsonDocument), Json, "BsonDocument.//address.street", "Baker street")]
        [InlineData("BsonDocument", typeof(BsonDocument), Json, "BsonDocument.//address.house", "5")]
        public void GetVariableValue_CorrectVariableBson_ReturnValue(string key, Type type, string value, string searchKey, string searchValue)
        {
            var doc
                = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(value);
            variableContext.SetVariable(key, type, doc);

            var variable = variableContext.GetVariableValue(searchKey);

            ((JValue)variable).Value.Should().Be(searchValue);
        }

        [Theory]
        [InlineData("second", typeof(string), "newsecond", TypeOfAccess.Global)]
        public void GetDoubleVariable_TypeOfAccessGlobal_ReturnException(string key, Type type, string value, TypeOfAccess typeOfAccess)
        {
            variableContext.SetVariable(key, type, value, typeOfAccess);

            Action act = () => variableContext.SetVariable(key, type, value, typeOfAccess);
            act
              .Should().Throw<ArgumentException>()
              .WithMessage($"Element with key: \"{key}\" has already created with type 'Global'");
        }        

        [Fact]
        public void GetVariableValue_SearchDataTable_ReturnValue()
        {
            var table = CreateObject.CreateDataTable(new List<string>() { "a", "b" }, new List<string>() { "a1;b1" });
            variableContext.SetVariable("DataTable", typeof(DataTable), table);

            var variable = variableContext.GetVariableValue("DataTable");
            (variable is DataTable).Should().BeTrue();
            variable.Should().Equals(table);
        }

        [Fact]
        public void GetVariableValue_SearchDataTableValueByNumber_ReturnValue()
        {
            var table = CreateObject.CreateDataTable(new List<string>() { "a", "b" }, new List<string>() { "a1;b1" });
            variableContext.SetVariable("DataTable", typeof(DataTable), table);

            var variable = variableContext.GetVariableValue("DataTable[0][0]");
            variable.Should().Be("a1");
        }

        [Fact]
        public void GetVariableValue_SearchDataTableValueByName_ReturnValue()
        {
            var table = CreateObject.CreateDataTable(new List<string>() { "a", "b" }, new List<string>() { "a1;b1" });
            variableContext.SetVariable("DataTable", typeof(DataTable), table);

            var variable = variableContext.GetVariableValue("DataTable[0][a]");
            variable.Should().Be("a1");
        }

        [Fact]
        public void GetVariableValue_SearchDataTableRow_ReturnValue()
        {
            var table = CreateObject.CreateDataTable(new List<string>() { "a", "b" }, new List<string>() { "a1;b1" });
            variableContext.SetVariable("DataTable", typeof(DataTable), table);

            var variable = variableContext.GetVariableValue("DataTable[0]");
            (variable is DataRow).Should().BeTrue();
        }       

        [Fact]
        public void GetVariableValue_VariableValueNullByName_ReturnNull()
        {
            variableContext.SetVariable("null", typeof(string), null);
            var variable = variableContext.GetVariableValue("null");

            variable.Should().BeNull();
        }

        [Fact]
        public void GetVariableValue_VariableValueNull_ReturnNull()
        {
            var variable = variableContext.GetVariableValue(null);

            variable.Should().BeNull();
        }

        [Fact]
        public void GetVariableValue_VariableNull_ReturnNull()
        {
            variableContext.Variables.TryAdd("null", null);
            var variable = variableContext.GetVariableValue("null");

            variable.Should().BeNull();
        }

        [Fact]
        public void GetVariableValue_SearchDataRow_ReturnValue()
        {
            var table = CreateObject.CreateDataTable(new List<string>() { "a", "b" }, new List<string>() { "a1;b1" });
            variableContext.SetVariable("DataRow", typeof(DataRow), table.Rows[0]);

            var variable = variableContext.GetVariableValue("DataRow");
            (variable is DataRow).Should().BeTrue();
            variable.Should().Equals(table.Rows[0]);
        }

        [Fact]
        public void GetVariableValue_SearchDataRowByNumber_ReturnValue()
        {
            var table = CreateObject.CreateDataTable(new List<string>() { "a", "b" }, new List<string>() { "a1;b1" });
            variableContext.SetVariable("DataRow", typeof(DataRow), table.Rows[0]);

            var variable = variableContext.GetVariableValue("DataRow[0]");
            variable.Should().Be("a1");
        }

        [Fact]
        public void GetVariableValue_SearchDataRowByName_ReturnValue()
        {
            var table = CreateObject.CreateDataTable(new List<string>() { "a", "b" }, new List<string>() { "a1;b1" });
            variableContext.SetVariable("DataRow", typeof(DataRow), table.Rows[0]);

            var variable = variableContext.GetVariableValue("DataRow[a]");
            variable.Should().Be("a1");
        }

        [Fact]
        public void GetVariableValue_SearchDataTableEmptyParam_ReturnNull()
        {
            var table = CreateObject.CreateDataTable(new List<string>() { "a", "b" }, new List<string>() { "a1;b1" });
            variableContext.SetVariable("DataTable", typeof(DataTable), table);

            var variable = variableContext.GetVariableValue("DataTable[]");
            variable.Should().BeNull();
        }


        [Theory]
        [InlineData("xml", typeof(XmlDocument), Xml, "xml")]
        [InlineData("xml", typeof(XmlDocument), Xml, "xml.//addresses")]
        [InlineData("xml", typeof(XmlDocument), Xml, "xml.//address")]
        public void GetVariableValueText_CorrectVariableXmlRoot_ReturnText(string key, Type type, string value, string searchKey)
        {
            var doc = new XmlDocument();
            doc.LoadXml(value);
            variableContext.SetVariable(key, type, doc);

            var variable = variableContext.GetVariableValueText(searchKey);
            variable.Should().BeNull();
        }

        [Theory]
        [InlineData("xml", typeof(XmlDocument), Xml, "xml.//street", "Baker street")]
        [InlineData("xml", typeof(XmlDocument), Xml, "xml.//house",  "5")]
        public void GetVariableValueText_CorrectVariableXml_ReturnText(string key, Type type, string value, string searchKey, string searchValue)
        {
            var doc = new XmlDocument();
            doc.LoadXml(value);
            variableContext.SetVariable(key, type, doc);

            var variable = variableContext.GetVariableValueText(searchKey);
            variable.Should().Be(searchValue);
        }

        [Theory]
        [InlineData("xDoc", typeof(XDocument), Xml, "xDoc",             "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<addresses xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"test.xsd\">\r\n  <address>\r\n    <name>Joe Tester</name>\r\n    <street>Baker street</street>\r\n    <house>5</house>\r\n  </address>\r\n</addresses>")]
        [InlineData("xDoc", typeof(XDocument), Xml, "xDoc.//addresses", "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<addresses xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"test.xsd\">\r\n  <address>\r\n    <name>Joe Tester</name>\r\n    <street>Baker street</street>\r\n    <house>5</house>\r\n  </address>\r\n</addresses>")]
        [InlineData("xDoc", typeof(XDocument), Xml, "xDoc.//address", "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<address>\r\n  <name>Joe Tester</name>\r\n  <street>Baker street</street>\r\n  <house>5</house>\r\n</address>")]
        public void GetVariableValueText_CorrectVariablexDocRoot_ReturnText(string key, Type type, string value, string searchKey, string expected)
        {
            var doc = XDocument.Parse(value);
            variableContext.SetVariable(key, type, doc);

            var variable = variableContext.GetVariableValueText(searchKey);
            variable.Should().Be(expected);
        }

        [Theory]
        [InlineData("xDoc", typeof(XDocument), Xml, "xDoc.//street", "Baker street")]
        [InlineData("xDoc", typeof(XDocument), Xml, "xDoc.//house", "5")]
        public void GetVariableValueText_CorrectVariablexDoc_ReturnText(string key, Type type, string value, string searchKey, string searchValue)
        {
            var doc = XDocument.Parse(value);
            variableContext.SetVariable(key, type, doc);

            var variable = variableContext.GetVariableValueText(searchKey);
            variable.Should().Be(searchValue);
        }

        [Theory]
        [InlineData("JObject", typeof(JObject), Json, "JObject.//address.street", "Baker street")]
        [InlineData("JObject", typeof(JObject), Json, "JObject.//address.house", "5")]
        [InlineData("JObject", typeof(JObject), Json, "JObject.//address", "{\r\n  \"name\": \"Joe Tester\",\r\n  \"street\": \"Baker street\",\r\n  \"house\": \"5\"\r\n}")]
        public void GetVariableValueText_CorrectVariableJObject_ReturnValue(string key, Type type, string value, string searchKey, string searchValue)
        {
            var doc = JObject.Parse(value);
            variableContext.SetVariable(key, type, doc);

            var variable = variableContext.GetVariableValueText(searchKey);

            variable.Should().Be(searchValue);
        }

        [Theory]
        [InlineData("JToken", typeof(JToken), Json, "JToken.//address.street", "Baker street")]
        [InlineData("JToken", typeof(JToken), Json, "JToken.//address.house", "5")]
        [InlineData("JToken", typeof(JToken), Json, "JToken.//address", "{\r\n  \"name\": \"Joe Tester\",\r\n  \"street\": \"Baker street\",\r\n  \"house\": \"5\"\r\n}")]
        public void GetVariableValueText_CorrectVariableJToken_ReturnValue(string key, Type type, string value, string searchKey, string searchValue)
        {
            var doc = JToken.Parse(value);
            variableContext.SetVariable(key, type, doc);

            var variable = variableContext.GetVariableValueText(searchKey);

            variable.Should().Be(searchValue);
        }

        [Theory]
        [InlineData("first", "1")]
        [InlineData("second", "2")]
        [InlineData("third", "3")]
        public void GetVariableValueText_CorrectVariable_ReturnValue(string key, string value)
        {
            var variable = variableContext.GetVariableValueText(key);

            variable.Should().Be(value);
        }

        [Fact]
        public void GetVariableValueText_VariableValueNullByName_ReturnNull()
        {
            variableContext.SetVariable("null", typeof(string), null);
            var variable = variableContext.GetVariableValueText("null");

            variable.Should().BeNull();
        }

        [Fact]
        public void GetVariableValueText_VariableValueNull_ReturnNull()
        {
            var variable = variableContext.GetVariableValueText(null);

            variable.Should().BeNull();
        }

        [Fact]
        public void GetVariableValueText_VariableNull_ReturnNull()
        {
            variableContext.Variables.TryAdd("null", null);
            var variable = variableContext.GetVariableValueText("null");

            variable.Should().BeNull();
        }

        [Fact]
        public void GetVariableValue_ListValue_ReturnList()
        {
            var list = new List<int> { 1, 2, 3 };
            var variable = new Variable
            {
                Type = list.GetType(),
                Value = list
            };
            variableContext.Variables.TryAdd("list", variable);

            var value = variableContext.GetVariableValue("list");
            value.Should().Be(list);
        }

        [Fact]
        public void GetVariableValue_ListValue_ReturnListValue()
        {
            int[] nums = { 1, 2, 3 };
            var variable = new Variable
            {
                Type = nums.GetType(),
                Value = nums
            };
            variableContext.Variables.TryAdd("nums", variable);

            var value = variableContext.GetVariableValue("nums[0]");
            value.Should().Be(1);
        }

        [Theory]
        [InlineData("")]
        public void GetVariableValueText_JsonSubObject_ReturnValue(string json)
        {
            
        }
    }
}