using EvidentInstruction.Config.Infrastructures;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace EvidentInstruction.Config.Models
{
    [ExcludeFromCodeCoverage]
    public class Config
    {
        [JsonProperty(Constants.CONFIG_BLOCK)]
        public List<Parameters> Parameters { get; set; }
    }
}
