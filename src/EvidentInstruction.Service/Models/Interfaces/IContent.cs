using System.Net.Http;

namespace EvidentInstruction.Service.Models.Interfaces
{
    public interface IContent
    {
        StringContent Get(HttpContent content);
    }
}
