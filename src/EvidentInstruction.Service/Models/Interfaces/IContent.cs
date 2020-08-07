using System.Net.Http;

namespace EvidentInstruction.Service.Models.Interfaces
{
    public interface IContent
    {
        HttpContent Get(string content);
    }
}
