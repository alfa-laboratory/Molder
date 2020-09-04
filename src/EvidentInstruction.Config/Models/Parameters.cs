using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace EvidentInstruction.Config.Models
{
    [ExcludeFromCodeCoverage]
    public class Parameters
    {
        [JsonProperty("tag")]
        public string Tag { get; set; }

        [JsonProperty("parameters")]
        public Dictionary<string, object> Param { get; set; }
    }
}
