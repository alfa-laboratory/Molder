using System.Collections.Generic;
using System.Net.Http;
using EvidentInstruction.Exceptions;
using EvidentInstruction.Service.Helpers;

namespace EvidentInstruction.Service.Models
{
     public class FlurlProvider: Interfaces.IServiceProvider
    {
        private const int timeoutDefault = 60;
        public ResponseInfo Send(RequestInfo request, HttpMethod method, Dictionary<string, string> headers, HttpContent content, int timeout = timeoutDefault)
        {

            try
            {
                var headerz = ReplaceHeaders.Replace(request.Headers, request.Content.ReadAsStringAsync().Result);
                var message = request.Url
                    .FlTimeout(request.Url, timeout)
                    .FlHeaders(headerz, request.Url)
                    .FlSend(request.Url, request.Method, request.Content);
                var response = new ResponseInfo()
                {
                    Content = ServiceHelpers.GetObjectFromString(message.Result.Content.ReadAsStringAsync().Result),
                    Headers = message.Result.Headers,
                    Request = request,
                    StatusCode = message.Result.StatusCode,
                    Exception = null
                };
                return response;
            }
            catch (WithHeadersException e)
            {
                return new ResponseInfo()
                {
                    Exception = e
                };
            }
            catch (ServiceException e)
            {
                return new ResponseInfo()
                {
                    Exception = e
                };
            }
            catch (ServiceTimeoutException e)
            {
                return new ResponseInfo()
                {
                    Exception = e
                };
            }
            catch (WithTimeoutException e)
            {
                return new ResponseInfo()
                {
                    Exception = e
                };
            }
        }
    }
}
