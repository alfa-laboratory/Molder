using System.Net.Http;
using System.Threading.Tasks;

namespace EvidentInstruction.Service.Models.Interfaces
{
    public interface IFlurlProvider
    {
        Task<HttpResponseMessage> SendRequest(RequestInfo request);
    }
}

