using System;
using System.Collections.Generic;
using System.Net.Http;
using EvidentInstruction.Exceptions;
using EvidentInstruction.Helpers;
using EvidentInstruction.Service.Helpers;
using Flurl.Http;
using TechTalk.SpecFlow;

namespace EvidentInstruction.Service.Models
{
     public class FlurlProvider: Interfaces.IServiceProvider
    {
        public HttpResponseMessage Send(RequestInfo request, HttpMethod method, Dictionary<string, string> headers, HttpContent content, int? timeout)
        {
            try
            {
                var headerz = ReplaceHeaders.Replace(request.Headers, request.Content.ReadAsStringAsync().Result);
                var message = request.Url
                    .FlTimeout((int)timeout)
                    .FlHeaders(headerz)
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

        public HttpResponseMessage Send(RequestInfo request, HttpMethod method, Dictionary<string, string> headers, HttpContent content, Table paramsValue, int? timeout)
        {
            try
            {
                var headerz = ReplaceHeaders.Replace(request.Headers, request.Content.ReadAsStringAsync().Result);
                var urlWithParams = request.Url
                    .FlSetParams(Converter.ConvertTable(paramsValue))
                    .ToString();
                var message = request.Url
                    .FlTimeout((int)timeout)
                    .FlHeaders(headerz)
                    .FlSend(urlWithParams, request.Method, request.Content);

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
