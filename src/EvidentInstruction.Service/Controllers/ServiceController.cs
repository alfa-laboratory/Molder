using EvidentInstruction.Service.Models;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace EvidentInstruction.Service.Controllers
{
    [ExcludeFromCodeCoverage]
    public class ServiceController
    {   
        public ConcurrentDictionary<string, ResponceInfo> Services { get; set; }

        public ServiceController()
        {
            Services = new ConcurrentDictionary<string, ResponceInfo>();
        }
    }
}
