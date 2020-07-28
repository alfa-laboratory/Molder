using EvidentInstruction.Service.Helpers;
using System.Net;
using System.Net.Http;
using EvidentInstruction.Exceptions;

namespace EvidentInstruction.Service.Models
{
    public static class ResponseExtenssions
    {
        public static ResponseInfo CreateResponse(this ResponseInfo r, HttpResponseMessage message)
        {

            r.Content = ServiceHelpers.GetObjectFromString(message.Content.ReadAsStringAsync().Result);
            r.Headers = message.Headers;
            r.StatusCode = HttpStatusCode.OK;

            return r;
        }

        public static ResponseInfo CreateResponse(this ResponseInfo r, ServiceException e)
        {
            r.Content = ServiceHelpers.GetObjectFromString(e.Message);
            r.StatusCode = HttpStatusCode.BadGateway;
            return r;
        }

        public static ResponseInfo CreateResponse(this ResponseInfo r, ServiceTimeoutException e)
        {
            r.Content = ServiceHelpers.GetObjectFromString(e.Message);
            r.StatusCode = HttpStatusCode.GatewayTimeout;
            return r;
        }

        public static ResponseInfo CreateResponse(this ResponseInfo r, WithTimeoutException e)
        {
            r.Content = ServiceHelpers.GetObjectFromString(e.Message);
            return r;
        }

        public static ResponseInfo CreateResponse(this ResponseInfo r, WithHeadersException e)
        {
            r.Content = ServiceHelpers.GetObjectFromString(e.Message);
            return r;
        }
    }
}
