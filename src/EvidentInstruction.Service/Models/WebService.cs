namespace EvidentInstruction.Service.Models
{
    public abstract class WebService
    {
        public abstract ResponseInfo SendMessage(RequestInfo request);
    }
}
