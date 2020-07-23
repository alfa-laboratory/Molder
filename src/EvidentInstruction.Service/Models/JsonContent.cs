using EvidentInstruction.Service.Models.Interfaces;

namespace EvidentInstruction.Service.Models
{
    public class JsonContent : IContent
    {
        public string Get(object content)
        {
            return ContentTypes.JSON;
        }
    }
}
