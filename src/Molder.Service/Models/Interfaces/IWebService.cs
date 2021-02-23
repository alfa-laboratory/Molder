
using System.Threading.Tasks;

namespace EvidentInstruction.Service.Models.Interfaces
{
    public interface IWebService
    {
        Task<ResponceInfo> SendMessage();
    }
}
