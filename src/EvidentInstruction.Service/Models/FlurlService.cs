using System;
using System.Net.Http;
using EvidentInstruction.Exceptions;
using EvidentInstruction.Helpers;
using EvidentInstruction.Service.Models.Interfaces;
using Flurl.Http;

namespace EvidentInstruction.Service.Models
{
    public class FlurlService : WebService, IWebService, IDisposable
    {
        public int? Timeout { get; set; }
        Interfaces.IServiceProvider flurProvider = new FlurlProvider();

        public FlurlService(int? timeout = null)
        {
            if (timeout == null) this.Timeout = Definitions.DefaultTimeout;
            else this.Timeout = timeout;
        }

        public override ResponseInfo SendMessage(RequestInfo request)
        {
            var (isValid, results) = Validate.ValidateModel(request);
            var response = new ResponseInfo();
            if (isValid)
            {
                try
                {
                    HttpResponseMessage result;
                    Log.Logger.Information(
                                  $"Сервисс с именем \"{request.Name}\" был вызван со следующими параметрами \"{request}\" ");
                    if (request.Params != null)
                    {
                        result = flurProvider.Send(request, request.Method, request.Headers, request.Content, request.Params, Timeout = this.Timeout);
                    }
                    else
                    {
                        result = flurProvider.Send(request, request.Method, request.Headers, request.Content, Timeout = this.Timeout);
                    }
                    response.CreateResponse(result);
                    return response;

                }
                catch (WithHeadersException e)
                {
                    response.CreateResponse(e);
                }
                catch (WithTimeoutException e)
                {
                    response.CreateResponse(e);
                }
                catch (ServiceTimeoutException e)
                {
                    response.CreateResponse(e);
                }
                catch (ServiceException e)
                {
                    response.CreateResponse(e); ;
                }
            }
            else
            {
                Log.Logger.Warning($"Модель для  вызова сервиса  с именем \"{request.Name}\" некорректна. Ошибки: \"{results}\" ");
                throw new ServiceException(call: new HttpCall(), $"Модель для  вызова сервиса  с именем \"{request.Name}\" некорректна. Ошибки: \"{results}\" ", null);
            }

            return response;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
