using System.Net.Http;
using System.Threading.Tasks;

namespace Molder.Service.Models
{
    public interface IFlurlProvider
    {
        Task<HttpResponseMessage> SendRequestAsync(RequestInfo request);
    }
}

