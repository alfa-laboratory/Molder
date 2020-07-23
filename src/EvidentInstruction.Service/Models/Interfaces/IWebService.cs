namespace EvidentInstruction.Service.Models.Interfaces
{
    interface IWebService
    {
        ResponseInfo SendMessage(RequestInfo request);
    }
}
