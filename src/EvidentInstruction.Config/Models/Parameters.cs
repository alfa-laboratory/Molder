using Newtonsoft.Json;
using System.Collections.Generic;

namespace EvidentInstruction.Config.Models
{
    public class Parameters
    {
        [JsonProperty("tag")]
        public string Tag { get; set; }

        [JsonProperty("parameters")]
        public Dictionary<string, object> Param { get; set; }
    }
}
