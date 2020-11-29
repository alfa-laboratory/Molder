using EvidentInstruction.Helpers;
using EvidentInstruction.Service.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace EvidentInstruction.Service.Helpers
{
    public static class ServiceHelpers
    {
        /// <summary>
        /// Определить к какому типу относится строка
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
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


        public static Dictionary<string, string> ReplaceHeaders(Dictionary<string, string> headers, string str, RequestInfo request) //не правильно работает. Сюда попадает контент и добавляет новый заголовок
        {
            var nHeaders = new Dictionary<string, string>();
            var contentType = string.Empty;
            /*var doc = ServiceHelpers.GetObjectFromString(str);*/
            object doc = request.Content.GetType();

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

            var key = headers.Select(x => x.Key).Where(y => y.ToLower().Contains("content-type")).First();

            if (!key.Any()) //учесть, что контент type, может быть написан с маленькой буквы. (Tolower или сравнить без регистра (ignorecase))
            {
                nHeaders = headers; //зачем приравниваем ссылку?
                nHeaders.Add("Content-Type", contentType);
            } //тернарный оператор?
            return headers;
        }
    }
}
