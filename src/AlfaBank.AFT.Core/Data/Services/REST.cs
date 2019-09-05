using AlfaBank.AFT.Core.Exceptions;
using AlfaBank.AFT.Core.Infrastructure.Common;
using AlfaBank.AFT.Core.Infrastructure.Service;
using System.Collections.Generic;
using System.Net;

namespace AlfaBank.AFT.Core.Data.Services
{
    public class Rest : Service
    {
        public Rest(string url, int? timeout = null, IWebProxy proxy = null, string userAgent = null, ICredentials credentials = null,
            int connectionLimit = 20,
            WebServiceMethod webServiceMethod = WebServiceMethod.POST)
        {
            Url = url;
            WebServiceMethod = webServiceMethod;
            Timeout = timeout ?? Timeout;
            Proxy = proxy;
            UserAgent = userAgent;
            ConnectionLimit = connectionLimit;
            Credentials = credentials;
        }

        public override (HttpStatusCode?, List<Error>) CallWebService(string body = null)
        {
            var (statusCode, errors) = SendHttpRequest(data: body);
            return (statusCode, errors);
        }
    }
}
