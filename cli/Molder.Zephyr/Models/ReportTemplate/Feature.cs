using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Molder.Zephyr.Models
{
    public class Feature
    {
        [JsonProperty("feature")]
        public string Name { get; set; }

        [JsonProperty("task")]
        public string Task { get; set; }
        
        public int IssueId { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("scenarios")]
        public IEnumerable<Scenario> Scenarios { get; set; }
    }

    public class Scenario
    {
        [JsonProperty("scenario")]
        public string Name { get; set; }
        
        [JsonProperty("orderId")]
        public int? OrderId { get; set; }
        
        public int TestStepId { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }
}