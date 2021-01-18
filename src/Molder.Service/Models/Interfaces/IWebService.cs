
namespace Molder.Service.Models.Interfaces
{
    public interface IWebService
    {
        ResponceInfo SendMessage(RequestInfo request/*, Dictionary<HTTPMethodType, HttpMethod> webMethods*/);
    }
}
