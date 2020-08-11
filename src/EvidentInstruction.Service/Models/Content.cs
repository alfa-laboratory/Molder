using System.Net.Http;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using EvidentInstruction.Service.Helpers;
using Newtonsoft.Json.Linq;

namespace EvidentInstruction.Service.Models
{
    public static class Content 
    {
        public static HttpContent Get(string content)
        {
            var type = ServiceHelpers.GetObjectFromString(content.GetType().ToString());
            switch (type)
            {
                case XmlDocument xml:
                case XDocument xdoc:
                {
                    return new StringContent((string)content, Encoding.UTF8, ContentTypes.XML);
                }
                case JObject json:
                {
                    return new StringContent((string)content, Encoding.UTF8, ContentTypes.JSON);
                }
                default:
                {
                    return new StringContent((string)content, Encoding.UTF8, ContentTypes.TEXT);
                }
            }
        }
    }
}
