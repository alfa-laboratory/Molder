using EvidentInstruction.Database.Models.Interfaces;
using System.Collections.Concurrent;

namespace EvidentInstruction.Database.Controllers
{
    public class DatabaseController
    {
        public ConcurrentDictionary<string, (IConnectionWrapper connection, int? timeout)> Connections { get; set; }

        public DatabaseController()
        {
            Connections = new ConcurrentDictionary<string, (IConnectionWrapper, int?)>();
        }
    }
}