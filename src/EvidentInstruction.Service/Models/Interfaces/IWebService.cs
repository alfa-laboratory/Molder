using EvidentInstruction.Service.Infrastructures;
using System.Collections.Generic;
using System.Net.Http;

namespace EvidentInstruction.Service.Models.Interfaces
{
    public interface IWebService
    {
        ResponceInfo SendMessage(RequestInfo request, Dictionary<HTTPMethodType, HttpMethod> webMethods);
    }
}
