using System.Net;
using System.Net.Http.Headers;

namespace EvidentInstruction.Service.Models
{
    public class ResponceInfo
    {
        public RequestInfo Request { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public HttpResponseHeaders Headers { get; set; }
        public object Content { get; set; }
    }
}