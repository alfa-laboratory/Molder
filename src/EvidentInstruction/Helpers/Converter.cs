using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using TechTalk.SpecFlow;

namespace EvidentInstruction.Helpers
{
    public static class Converter
    {
        public static IEnumerable CreateEnumerable(string str, string splitChars)
        {
            try
            { 
                var arrayStrings = str.Split(splitChars.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                var enumerable = arrayStrings.Select(word => word.Trim()).ToArray();
                return enumerable;
            }
            catch(NullReferenceException)
            {
                if (string.IsNullOrEmpty(str))
                {
                    Log.Logger.Warning("Входная строка для создания списка не задана");
                    return null;
                }
                else
                {
                    Log.Logger.Warning("Входная строка для создания списка не задана");
                    return new List<string>() { str };
                }
            }
        }

        public static XDocument CreateXDoc(string str)
        {
            try
            {
                return XDocument.Parse(str);
            }
            catch (XmlException ex)
            {
                Log.Logger.Warning(ex.Message);
                return null;
            }
        }

        public static XmlDocument CreateXmlDoc(string str)
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(str);
                return doc;
            }catch(XmlException ex)
            {
                Log.Logger.Warning(ex.Message);
                return null;
            }
        }

        public static JObject CreateJson(string str)
        {
            try
            {
                return JObject.Parse(str);
            }catch(JsonException ex)
            {
                Log.Logger.Warning(ex.Message);
                return null;
            }
        }

        public static XDocument CreateCData(XElement xElement)
        {
            try
            {
                var data = ((XText)xElement.FirstNode).Value;
                var xDocument = XDocument.Parse(data);
                return xDocument;
            }
            catch(ArgumentNullException)
            {
                Log.Logger.Warning($"Значение переменной для преобразования в CDATA равно null");
                return null;
            }
            catch (XmlException ex)
            {
                Log.Logger.Warning(ex.Message);
                return null;
            }
        }

        public static string[] ConvertTable(Table table)
        {
            var list = new List<string>();
            int pointer = 0;
            foreach (var head in table.Header)
            {
                foreach (var row in table.Rows)
                {
                    list.Add($"{head}={row[pointer]}");
                }

                pointer++;
            }

            return list.ToArray();
        }
    }
}
