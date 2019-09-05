using AlfaBank.AFT.Core.Exceptions;
using AlfaBank.AFT.Core.Infrastructure.Service;
using System.Collections.Generic;
using System.Net;

namespace AlfaBank.AFT.Core.Data.Services
{
    public class Iis : Service
    {
        public Iis(string url, WebServiceMethod method, int? timeout = null)
        {
            Url = url;
            Method = method;
            Timeout = timeout ?? Timeout;
        }

        public new int? Timeout { get; private set; } = 60000;

        public WebServiceMethod Method
        {
            get; private set;
        }

        public override (HttpStatusCode?, List<Error>) CallWebService(string body = null)
        {
            var (statusCode, errors) = SendHttpRequest(data: body);

            return (statusCode, errors);
        }
    }
}
