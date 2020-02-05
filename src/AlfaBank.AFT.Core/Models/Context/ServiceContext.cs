using AlfaBank.AFT.Core.Models.Service;
using System.Collections.Generic;

namespace AlfaBank.AFT.Core.Models.Context
{
    public class ServiceContext
    {
        public Dictionary<string, WebService> Services { get; set; }
    }
}