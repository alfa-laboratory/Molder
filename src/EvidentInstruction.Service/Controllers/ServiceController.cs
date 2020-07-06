using EvidentInstruction.Service.Models;
using System.Collections.Concurrent;

namespace EvidentInstruction.Service.Controllers
{
    public class ServiceController
    {   
        public ConcurrentDictionary<string, ResponceInfo> Services { get; set; }

        public ServiceController()
        {
            Services = new ConcurrentDictionary<string, ResponceInfo>();
        }
    }
}
