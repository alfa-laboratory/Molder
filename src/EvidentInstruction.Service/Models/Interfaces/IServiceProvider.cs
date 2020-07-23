using System.Net.Http;
using System.Threading.Tasks;

namespace EvidentInstruction.Service.Models.Interfaces
{
    public interface IServiceProvider
    {
        Task<HttpResponseMessage> response { get; set; }
        (bool, ResponseInfo) WrapMethod(RequestInfo request);
    }
}
