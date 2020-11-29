
using EvidentInstruction.Service.Infrastructures;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace EvidentInstruction.Service.Models.Interfaces
{
    public interface IServiceFlurlProvider
    {
        Task<HttpResponseMessage> SendAsync(RequestInfo request, Dictionary<string, string> dic1, HTTPMethodType type);


    }
}
