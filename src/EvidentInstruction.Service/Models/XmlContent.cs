using EvidentInstruction.Service.Models.Interfaces;

namespace EvidentInstruction.Service.Models
{
    public class XmlContent : IContent
    {
        public string Get(object content)
        {
            return ContentTypes.XML;
        }
    }
}
