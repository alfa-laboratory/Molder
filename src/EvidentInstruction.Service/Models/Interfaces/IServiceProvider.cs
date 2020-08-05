using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace EvidentInstruction.Service.Models.Interfaces
{
    public interface IServiceProvider
    {
        HttpResponseMessage Send(RequestInfo request, HttpMethod method, Dictionary<string, string> headers,
            HttpContent content, int? timeout);
        HttpResponseMessage Send(RequestInfo request, HttpMethod method, Dictionary<string, string> headers,
            HttpContent content, Dictionary<string, string> paramsValue, int? timeout);
    }
}
