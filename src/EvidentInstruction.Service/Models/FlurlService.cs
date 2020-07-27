using System;
using EvidentInstruction.Exceptions;
using EvidentInstruction.Helpers;
using EvidentInstruction.Service.Models.Interfaces;
using IServiceProvider = EvidentInstruction.Service.Models.Interfaces.IServiceProvider;

namespace EvidentInstruction.Service.Models
{
    public class FlurlService : WebService, IWebService, IDisposable
    {
        public int Timeout { get; set; }

        IServiceProvider flurProvider = new FlurlProvider();

        public FlurlService(int timeout = 60)
        {
            this.Timeout = timeout;
        }

        public override ResponseInfo SendMessage(RequestInfo request)
        {
            var (isValid, results) = Validate.ValidateModel(request);
            var response = new ResponseInfo();
            if (isValid)
            {
                Log.Logger.Information(
                    $"Сервисс с именем \"{request.Name}\" был вызван со следующими параметрами \"{request}\" ");
                var result = flurProvider.Send(request, request.Method, request.Headers, request.Content, Timeout);
                switch (result.Exception)
                {
                    case WithHeadersException e:
                        {
                            return response.CreateResponse(result);
                            Log.Logger.Warning(
                                $"Результат вызова сервиса  с именем \"{request.Name}\" закончился с ошибкой \"{e.Message}\" ");
                            break;
                        }
                    case ServiceException e:
                        {
                            return response.CreateResponse(result);
                            Log.Logger.Warning(
                                $"Результат вызова сервиса  с именем \"{request.Name}\" закончился с ошибкой \"{e.Message}\" ");
                            break;
                        }
                    case ServiceTimeoutException e:
                        {
                            return response.CreateResponse(result);
                            Log.Logger.Warning(
                                $"Результат вызова сервиса  с именем \"{request.Name}\" закончился с ошибкой \"{e.Message}\" ");
                            break;
                        }
                    case WithTimeoutException e:
                        {
                            return response.CreateResponse(result);
                            Log.Logger.Warning(
                                $"Результат вызова сервиса  с именем \"{request.Name}\" закончился с ошибкой \"{e.Message}\" ");
                            break;
                        }
                    default:
                        {
                            return response.CreateResponse(result);
                            Log.Logger.Information($"Результат вызова сервиса  с именем \"{request.Name}\" Результат: \"{result}\" ");
                            break;
                        }

                }

            }
            else
            {
                Log.Logger.Warning($"Модель для  вызова сервиса  с именем \"{request.Name}\" некорректна Ошибки: \"{results}\" ");
                throw new ServiceException($"Модель для  вызова сервиса  с именем \"{request.Name}\" некорректна Ошибки: \"{results}\" ");
            }
        }



        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
