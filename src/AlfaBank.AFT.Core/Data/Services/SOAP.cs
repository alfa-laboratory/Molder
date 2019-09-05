using AlfaBank.AFT.Core.Exceptions;
using AlfaBank.AFT.Core.Infrastructure.Service;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;

namespace AlfaBank.AFT.Core.Data.Services
{
    public class Soap : Service
    {
        public Soap(string url, string serviceMethod, int? timeout = null, IWebProxy proxy = null, string userAgent = null,
            int connectionLimit = 20,
            WebServiceMethod webServiceMethod = WebServiceMethod.POST)
        {
            Url = url;
            WebServiceMethod = webServiceMethod;
            Headers = new NameValueCollection()
            {
                {
                    "SOAPAction",
                    serviceMethod
                },
                {
                    "Content-Type",
                    @"text/xml"
                },
                {
                    "charset",
                    "UTF-8"
                },
            };
            Timeout = timeout ?? Timeout;
            Proxy = proxy;
            UserAgent = userAgent;
            ConnectionLimit = connectionLimit;
        }

        public override (HttpStatusCode?, List<Error>) CallWebService(string body = null)
        {
            var (statusCode, errors) = SendHttpRequest(data: body);
            return (statusCode, errors);
        }
    }
}
