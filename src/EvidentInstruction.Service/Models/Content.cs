using System.Net.Http;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using EvidentInstruction.Service.Helpers;
using EvidentInstruction.Service.Models.Interfaces;
using Newtonsoft.Json.Linq;

namespace EvidentInstruction.Service.Models
{
    public class Content : IContent
    {
        public StringContent Get(HttpContent content)
        {
            var type = ServiceHelpers.GetObjectFromString(content.GetType().ToString());
            switch (type)
            {
                case XmlDocument xml:
                case XDocument xdoc:
                {
                    return new StringContent((nameof(content)), Encoding.UTF8, ContentTypes.XML);
                }
                case JObject json:
                {
                    return new StringContent((nameof(content)), Encoding.UTF8, ContentTypes.JSON);
                }
                default:
                {
                    return new StringContent((nameof(content)), Encoding.UTF8, ContentTypes.TEXT);
                }
            }
        }
    }
}
