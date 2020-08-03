using System;
using System.Net.Http;
using EvidentInstruction.Exceptions;
using EvidentInstruction.Helpers;
using EvidentInstruction.Service.Models.Interfaces;
using Flurl.Http;
using TechTalk.SpecFlow;

namespace EvidentInstruction.Service.Models
{
    public class FlurlService : WebService, IWebService, IDisposable
    {
        public int? Timeout { get; set; }
        public Table paramsValue { get; set; }

        Interfaces.IServiceProvider flurProvider = new FlurlProvider();

        public FlurlService(int? timeout = null, Table paramsValue = null)
        {
            if (timeout == null) this.Timeout = Definitions.DefaultTimeout;
            else this.Timeout = timeout;
            this.paramsValue = paramsValue;
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
                    if (paramsValue != null) result = flurProvider.Send(request, request.Method, request.Headers, request.Content, paramsValue = this.paramsValue, Timeout = this.Timeout);
                    else result = flurProvider.Send(request, request.Method, request.Headers, request.Content, Timeout = this.Timeout);
                    response.CreateResponse(result);
                    return response;

                }
                catch (WithHeadersException e)
                {
                    response.CreateResponse(e);
                    return response;
                }
                catch (WithTimeoutException e)
                {
                    response.CreateResponse(e);
                    return response;
                }
                catch (ServiceTimeoutException e)
                {
                    response.CreateResponse(e);
                    return response;
                }
                catch (ServiceException e)
                {
                    response.CreateResponse(e);
                    return response;
                }
            }
            else
            {
                Log.Logger.Warning($"Модель для  вызова сервиса  с именем \"{request.Name}\" некорректна. Ошибки: \"{results}\" ");
                throw new ServiceException(call: new HttpCall(), $"Модель для  вызова сервиса  с именем \"{request.Name}\" некорректна. Ошибки: \"{results}\" ", null);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
