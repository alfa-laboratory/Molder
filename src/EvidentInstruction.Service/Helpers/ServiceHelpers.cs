using EvidentInstruction.Helpers;
using EvidentInstruction.Service.Infrastructures;
using EvidentInstruction.Service.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace EvidentInstruction.Service.Helpers
{
    public static class ServiceHelpers
    {
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

        public static string AddQueryInURL(string url, string query)
        {
           return query.StartsWith("?")? url + query: url + "?" + query;
        }

        /// <summary>
        /// Получить StringContent для RequestInfo 
        /// </summary>   
        public static StringContent GetStringContent(object type, string replaceContent)
        {
            StringContent stringContent = null;

            switch (type)
            {
                case XDocument xDoc:
                case XmlDocument xmlDocument:
                    {
                        stringContent = new StringContent(replaceContent, Encoding.UTF8, DefaultContentType.XML);
                        break;
                    }
                case JObject jObject:
                    {
                        stringContent = new StringContent(replaceContent, Encoding.UTF8, DefaultContentType.JSON);
                        break;
                    }
                default:
                    {
                        stringContent = new StringContent(replaceContent, Encoding.UTF8, DefaultContentType.TEXT);
                        break;
                    }
            }

            return stringContent;
        }


        /// <summary>
        /// Задать
        /// </summary>        
        public static Dictionary<string, string> ReplaceHeaders(Dictionary<string, string> headers, RequestInfo request) //TODO если  HEADERS TYPE EMPTY или убрать !!
        {
            var nHeaders = new Dictionary<string, string>();
            var contentType = string.Empty;
            /*var doc = ServiceHelpers.GetObjectFromString(str);*/
            object doc = request.Headers.GetType();// тут не контент

            switch (doc)
            {
                case XmlDocument xmlDoc:
                case XDocument xDoc:
                    {
                        contentType = "text/xml";
                        break;
                    }
                case JObject jObject:
                    {
                        contentType = "application/json";
                        break;
                    }
                default:
                    {
                        contentType = "text/plain";
                        break;
                    }
            }

           // var key = headers.ContainsKey(headers.Select(x => x.Key).Where(y => y.ToLower().Contains("content-type")).First()); //тут будет ошибка, если контент не так написан

            if (headers.ContainsKey(""))
            {
                nHeaders = headers; 
                nHeaders.Add("Content-Type", contentType);
            } 
            return nHeaders;
        }
    }
}
