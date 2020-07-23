using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace EvidentInstruction.Config.Models
{
    [ExcludeFromCodeCoverage]
    public class Config
    {
        [JsonProperty("config")]
        public Parameters[] Parameters { get; set; }
    }
}
