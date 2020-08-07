using System;
using System.Net.Http;
using EvidentInstruction.Exceptions;
using EvidentInstruction.Helpers;
using EvidentInstruction.Service.Models.Interfaces;
using Flurl.Http;
using IServiceProvider = EvidentInstruction.Service.Models.Interfaces.IServiceProvider;

namespace EvidentInstruction.Service.Models
{
    public class FlurlService : WebService, IWebService, IDisposable
    {
        public ServiceAttribute ServiceAttribute { get; set; } = new ServiceAttribute();
        IServiceProvider flurProvider = new FlurlProvider();

        public FlurlService(ServiceAttribute parameters)
        {
            if (parameters.Timeout == null)
            {
                ServiceAttribute.Timeout = Definitions.DefaultTimeout;
            }
            else
            {
                ServiceAttribute.Timeout = parameters.Timeout;
            }
            ServiceAttribute.Headers = parameters.Headers;
            ServiceAttribute.Parameters = parameters.Parameters;
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
                    Log.Logger.Information(Message.CreateMessage(request));
                    if (request.ServiceAttribute.Parameters != null)
                    {
                        result = flurProvider.Send(request, request.Method, request.ServiceAttribute.Headers, request.Content, request.ServiceAttribute.Parameters, ServiceAttribute.Timeout = this.ServiceAttribute.Timeout);
                    }
                    else
                    {
                        result = flurProvider.Send(request, request.Method, request.ServiceAttribute.Headers, request.Content, ServiceAttribute.Timeout = this.ServiceAttribute.Timeout);
                    }
                    response.CreateResponse(result);
                    Log.Logger.Information(Message.CreateMessage(response));
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
