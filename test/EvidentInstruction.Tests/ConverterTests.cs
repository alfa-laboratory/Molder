using FluentAssertions;
using EvidentInstruction.Helpers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace EvidentInstruction.Tests
{
    /// <summary>
    /// Тесты проверки конвертаций.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ConverterTests 
    {
        private const string Xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><addresses xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation='test.xsd'><address><name>Joe Tester</name><street>Baker street</street><house>5</house></address></addresses>";
        private const string Json = "{\"address\": {\"name\": \"Joe Tester\",\"street\": \"Baker street\",\"house\": \"5\" }}";
        private const string Cdata = "<addresses><![CDATA[<address><name>Joe Tester</name><street>Baker street</street><house>5</house></address>]]></addresses>";

        public static IEnumerable<object[]> ConvertedData()
        {
            yield return new object[] { "abc", "", new List<string> { "abc" } };
            yield return new object[] { "a,bc", ",", new List<string> { "a", "bc" } };
            yield return new object[] { ",abc", ",", new List<string> { "abc" } };
            yield return new object[] { "abc,", ",", new List<string> { "abc" } };
            yield return new object[] { "ab,c", ",", new List<string> { "ab", "c" } };
            yield return new object[] { "abc", null, new List<string> { "abc" } };
            yield return new object[] { null, ",", null };
            yield return new object[] { null, null, null };
        }

        [Theory]
        [MemberData(nameof(ConvertedData))]
        public void CreateEnumerable_CorrectParams_ReturnEnumerable(string str, string chars, List<string> list)
        {
            var convertedList = Converter.CreateEnumerable(str, chars);
            convertedList.Should().Equal(list);
        }

        [Fact]
        public void CreateXDoc_ValidXml_ReturnXDoc()
        {
            var xDoc = Converter.CreateXDoc(Xml);
            xDoc.Should().NotBeNull();
        }

        [Fact]
        public void CreateXDoc_NotValidXml_ReturnNull()
        {
            var xDoc = Converter.CreateXDoc("test");
            xDoc.Should().BeNull();
        }

        [Fact]
        public void CreateXmlDoc_ValidXml_ReturnXmlDoc()
        {
            var xml = Converter.CreateXmlDoc(Xml);
            xml.Should().NotBeNull();
        }

        [Fact]
        public void CreateXmlDoc_NotValidXml_ReturnNull()
        {
            var xml = Converter.CreateXmlDoc("test");
            xml.Should().BeNull();
        }

        [Fact]
        public void CreateJson_ValidJson_ReturnJObject()
        {
            var json = Converter.CreateJson(Json);
            json.Should().NotBeNull();
        }

        [Fact]
        public void CreateJson_NotValidJson_ReturnNull()
        {
            var json = Converter.CreateJson("test");
            json.Should().BeNull();
        }

        [Fact]
        public void CreateXmlCData_ValidXml_ReturnXml()
        {
            var cdata = Converter.CreateCData(Cdata);
            cdata.Should().NotBeNull();
        }

        [Fact]
        public void CreateXmlCData_NotValidXml_ReturnNull()
        {
            var cdata = Converter.CreateCData("test");
            cdata.Should().BeNull();
        }

        [Fact]
        public void CreateXmlCData_NotValidCData_ReturnNull()
        {
            var cdata = Converter.CreateCData(Xml);
            cdata.Should().BeNull();
        }
    }
}
