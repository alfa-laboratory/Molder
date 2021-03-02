using System.Threading.Tasks;

namespace Molder.Service.Models.Interfaces
{
    public interface IWebService
    {
        Task<ResponceInfo> SendMessage(RequestInfo requestInfo);
    }
}
