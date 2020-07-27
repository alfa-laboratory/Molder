using System;
using System.Net;
using System.Net.Http.Headers;

namespace EvidentInstruction.Service.Models
{
    public class ResponseInfo
    {
        public RequestInfo Request { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public HttpResponseHeaders Headers { get; set; }
        public object Content { get; set; }
        public Exception Exception { get; set; }
    }
}