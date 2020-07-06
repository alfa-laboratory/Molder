using Newtonsoft.Json;

namespace AlfaBank.AFT.Core.Models.Config
{
    public class Config
    {
        [JsonProperty("reports")]
        public string[] Reports { get; set; }

        [JsonProperty("parameters")]
        public Parameter[] Parameters { get; set; }
    }
}