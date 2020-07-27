using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using EvidentInstruction.Service.Models;
using Newtonsoft.Json.Linq;

namespace EvidentInstruction.Service.Helpers
{
    public static class ReplaceHeaders
    {
        public static Dictionary<string, string> Replace(Dictionary<string, string> headers, string str)
        {
            var nHeaders = new Dictionary<string, string>();
            var contentType = string.Empty;
            var doc = ServiceHelpers.GetObjectFromString(str);

            switch (doc)
            {
                case XmlDocument xmlDoc:
                case XDocument xDoc:
                {
                    contentType = ContentTypes.XML;
                    break;
                }
                case JObject jObject:
                {
                    contentType = ContentTypes.JSON;
                    break;
                }
                default:
                {
                    contentType = ContentTypes.TEXT;
                    break;
                }
            }

            if (!headers.ContainsKey("Content-Type"))
            {
                nHeaders = headers;
                nHeaders.Add("Content-Type", contentType);
            }
            return headers;
        }
    }
}
