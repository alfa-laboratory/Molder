using EvidentInstruction.Service.Models;
using Flurl.Http;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace EvidentInstruction.Service.Extension
{
    public static class FlurlExtension
    {
#if false
        public static IFlurlRequest Auth(this IFlurlRequest req, RequestInfo request, string login, string password) 
        {
            ((HttpClientHandler)request.Url.WithClient(req.Client)).Credentials = new NetworkCredential(login, password);

            return req;
        }

        public static IFlurlRequest WithCreditians(this string url, RequestInfo request)
        {
            var key = request.Headers.Select(x => x.Key).Where(y => y.ToLower().Contains("auth")).First();
            var login = request.Headers[key].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            return new FlurlRequest(url).Auth(request, login[0], login[1]);
        }
#endif
    }
}
