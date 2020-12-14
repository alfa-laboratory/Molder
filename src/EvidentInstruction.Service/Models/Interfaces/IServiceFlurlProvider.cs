using EvidentInstruction.Service.Infrastructures;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace EvidentInstruction.Service.Models.Interfaces
{
    public interface IServiceFlurlProvider
    {
        Task<HttpResponseMessage> SendAsync(RequestInfo request, Dictionary<string, string> headers, HTTPMethodType type);
    }
}
