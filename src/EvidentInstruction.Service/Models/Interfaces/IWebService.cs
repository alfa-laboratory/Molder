
namespace EvidentInstruction.Service.Models.Interfaces
{
    public interface IWebService
    {
        ResponceInfo SendMessage(RequestInfo request/*, Dictionary<HTTPMethodType, HttpMethod> webMethods*/);
    }
}
