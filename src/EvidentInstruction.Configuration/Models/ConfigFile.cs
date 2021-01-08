using System.Collections.Generic;

namespace EvidentInstruction.Configuration.Models
{
    public class ConfigFile
    {
        public string Tag { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
    }
}
