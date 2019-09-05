using AlfaBank.AFT.Core.Model.Service;
using System.Collections.Generic;

namespace AlfaBank.AFT.Core.Model.Context
{
    public class ServiceContext
    {
        public Dictionary<string, WebService> Services { get; set; }
    }
}