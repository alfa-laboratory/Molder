﻿using FluentAssertions;
using Molder.Helpers;
using Molder.Service.Infrastructures;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
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
        public static object GetObject(this string str)
        {   
            var xDoc = Converter.CreateXDoc(str);
            if (xDoc != null) return xDoc;
            
            var xmlDoc = Converter.CreateXmlDoc(str);
            if (xmlDoc != null) return xmlDoc;
            
            var jsonDoc = Converter.CreateJson(str);
            
            return jsonDoc ?? (object) str;
        }

        /// <summary>
        /// Добавить query к url
        /// </summary>        
        public static string AddQueryInURL(this string url, Dictionary<string, string> query)
        {
            url.Should().NotBeNull("web service address not specified");
            query.Should().NotBeNull("web service query not specified");
            var queryString = string.Join("&", query.Select(kv => kv.Key + "=" + kv.Value).ToArray());
            return url.Contains("?") ? $"{url}&{queryString}" : $"{url}?{queryString}";
        }

        /// <summary>
        /// Получить StringContent для RequestInfo 
        /// </summary>   
        public static HttpContent GetHttpContent(this object obj, string content)
        {    
            switch (obj)
            {
                case XDocument _:
                case XmlDocument _:
                    {
                        return new StringContent(content, Encoding.UTF8, DefaultContentType.XML);                        
                    }
                case JObject _:
                    {
                        return new StringContent(content, Encoding.UTF8, DefaultContentType.JSON);                        
                    }
                default:
                    {
                        return new StringContent(content, Encoding.UTF8, DefaultContentType.TEXT);                        
                    }
            }
        }
    }
}
