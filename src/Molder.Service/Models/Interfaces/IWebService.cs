
namespace Molder.Service.Models
{
    public interface IWebService
    {
        ResponceInfo SendMessage(RequestInfo request/*, Dictionary<HTTPMethodType, HttpMethod> webMethods*/);
    }
}
