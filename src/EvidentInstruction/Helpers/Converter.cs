using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

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
                    Log.Logger.Warning("Input string for creating the list is not specified");
                    return null;
                }
                else
                {
                    Log.Logger.Warning("Input string for creating the list is not specified");
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

        public static XDocument CreateCData(string str)
        {
            try
            {
                var element = XElement.Parse(str);
                if(element.FirstNode.NodeType != XmlNodeType.CDATA)
                {
                    Log.Logger.Warning($"The variable value to convert to CDATA is null");
                    return null;
                }
                var data = ((XText)element.FirstNode).Value;
                var xDocument = XDocument.Parse(data);
                return xDocument;
            }
            catch (XmlException ex)
            {
                Log.Logger.Warning($"Variable value to convert to CDATA was not created due to error: {ex.Message}");
                return null;
            }
        }
    }
}
