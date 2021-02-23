using System.Net.Http;
using System.Threading.Tasks;

namespace Molder.Service.Models
{
    public interface IFlurlProvider
    {
        Task<HttpResponseMessage> SendRequest(RequestInfo request);
    }
}

