using Molder.Service.Models;
using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Molder.Service.Controllers
{
    [ExcludeFromCodeCoverage]
    public class ServiceController
    {
        private Lazy<ConcurrentDictionary<string, ResponceInfo>> _services = new(() => new ConcurrentDictionary<string, ResponceInfo>());

        public ConcurrentDictionary<string, ResponceInfo> Services => _services.Value;
    }
}
