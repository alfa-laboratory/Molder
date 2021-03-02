using Molder.Helpers;
using Molder.Service.Infrastructures;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Molder.Service.Helpers
{
    public static class ServiceHelpers
    {
        public static JToken ToJson(this object obj)
        {
            return JToken.Parse(obj.ToString());
        }

        public static XDocument ToXml(this object obj)
        {
            var xmlDoc = XDocument.Parse(obj.ToString());
            return xmlDoc;
        }

        /// <summary>
        /// Определить к какому типу относится строка
        /// </summary>     
        public static object GetObjectFromString(string str)
        {   
            var xDoc = Converter.CreateXDoc(str);            
            if (xDoc == null)
            {
                var xmlDoc = Converter.CreateXmlDoc(str);
                if (xmlDoc == null)
                {
                    var jsonDoc = Converter.CreateJson(str);
                    if (jsonDoc == null)
                    {
                        return str;
                    }
                    else
                    {
                        return jsonDoc;
                    }
                }
                else
                {
                    return xmlDoc;
                }
            }
            else
            {
                return xDoc;
            }
        }

        /// <summary>
        /// Добавить query к url
        /// </summary>        
        public static string AddQueryInURL(this string url, string query)
        {
           return query.StartsWith("?")? url + query: url + "?" + query;
        }

        /// <summary>
        /// Получить StringContent для RequestInfo 
        /// </summary>   
        public static StringContent GetStringContent(object type, string replaceContent)
        {    
            switch (type)
            {
                case XDocument xDoc:
                case XmlDocument xmlDocument:
                    {
                        return new StringContent(replaceContent, Encoding.UTF8, DefaultContentType.XML);                        
                    }
                case JObject jObject:
                    {
                        return new StringContent(replaceContent, Encoding.UTF8, DefaultContentType.JSON);                        
                    }
                default:
                    {
                        return new StringContent(replaceContent, Encoding.UTF8, DefaultContentType.TEXT);                        
                    }
            }
        }
    }
}
