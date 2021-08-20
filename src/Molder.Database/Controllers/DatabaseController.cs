using Molder.Database.Models;
using Molder.Infrastructures;
using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Molder.Database.Controllers
{
    [ExcludeFromCodeCoverage]
    public class DatabaseController
    {
        private Lazy<ConcurrentDictionary<string, (IDbClient connection, TypeOfAccess typeOfAccess, int? timeout)>> _connections = new(() => new ConcurrentDictionary<string, (IDbClient connection, TypeOfAccess typeOfAccess, int? timeout)>());

        public ConcurrentDictionary<string, (IDbClient connection, TypeOfAccess typeOfAccess, int? timeout)> Connections => _connections.Value;
    }
}