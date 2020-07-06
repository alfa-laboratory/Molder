using System.Collections.Generic;
using Newtonsoft.Json;

namespace AlfaBank.AFT.Core.Models.Config
{
    public class Parameter
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("params")]
        public Dictionary<string, object> Params { get; set; }
    }
}
