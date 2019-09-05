using AlfaBank.AFT.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;

namespace AlfaBank.AFT.Core.Data.Services
{
    internal interface IService : IDisposable
    {
        (HttpStatusCode?, List<Error>) CallWebService(string body = null);
    }
}
