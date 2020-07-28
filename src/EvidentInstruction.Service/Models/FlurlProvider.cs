using System;
using System.Collections.Generic;
using System.Net.Http;
using EvidentInstruction.Exceptions;
using EvidentInstruction.Service.Helpers;
using Flurl.Http;

namespace EvidentInstruction.Service.Models
{
     public class FlurlProvider: Interfaces.IServiceProvider
    {
        public HttpResponseMessage Send(RequestInfo request, HttpMethod method, Dictionary<string, string> headers, HttpContent content, int timeout = Definitions.DefaultTimeout)
        {

            try
            {
                var headerz = ReplaceHeaders.Replace(request.Headers, request.Content.ReadAsStringAsync().Result);
                var message = request.Url
                    .FlTimeout(request.Url, timeout)
                    .FlHeaders(headerz, request.Url)
                    .FlSend(request.Url, request.Method, request.Content);
              
                return message.Result;
            }
            catch (AggregateException e)
            {
                throw new ServiceException(call: new HttpCall(), e.Message, e);
            }
            catch (WithHeadersException e)
            {
                throw new WithHeadersException(e.Message, e);
            }
            catch (ServiceTimeoutException e)
            {
                throw new ServiceTimeoutException(call: new HttpCall(), e.Message, e);
            }
            catch (ServiceException e)
            {
                throw new ServiceException(call: new HttpCall(), e.Message, e);
            }
            catch (WithTimeoutException e)
            {
                throw new WithTimeoutException(e.Message, e);
            }
        }
    }
}
