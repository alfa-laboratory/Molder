using Molder.Database.Models;
using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Molder.Database.Controllers
{
    [ExcludeFromCodeCoverage]
    public class DatabaseController
    {
        private Lazy<ConcurrentDictionary<string, (IDbClient connection, int? timeout)>> _connections = new Lazy<ConcurrentDictionary<string, (IDbClient connection, int? timeout)>>(() => new ConcurrentDictionary<string, (IDbClient connection, int? timeout)>());

        public ConcurrentDictionary<string, (IDbClient connection, int? timeout)> Connections
        {
            get => _connections.Value;
        }
    }
}