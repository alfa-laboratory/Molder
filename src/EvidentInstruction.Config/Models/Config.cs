using Newtonsoft.Json;

namespace EvidentInstruction.Config.Models
{
   public class Config
    {
        [JsonProperty("config")]
        public Parameters[] Parameters { get; set; }
    }
}
