using System.Collections.Generic;

namespace EvidentInstruction.Service.Models
{
    public class ServiceAttribute
    {
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public int? Timeout { get; set; }
    }
}
