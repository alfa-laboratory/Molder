using EvidentInstruction.Database.Models.Interfaces;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace EvidentInstruction.Database.Controllers
{
    [ExcludeFromCodeCoverage]
    public class DatabaseController
    {
        public ConcurrentDictionary<string, (IDbClient connection, int? timeout)> Connections { get; set; }

        public DatabaseController()
        {
            Connections = new ConcurrentDictionary<string, (IDbClient, int?)>();
        }
    }
}