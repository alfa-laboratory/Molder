using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace EvidentInstruction.Service.Models.Interfaces
{
    public interface IServiceProvider
    {
        HttpResponseMessage Send(RequestInfo request, HttpMethod method, Dictionary<string, string> headers,
            HttpContent content, int timeout);
    }
}
