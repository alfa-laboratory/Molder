using Flurl.Http;
using EvidentInstruction.Helpers;
using EvidentInstruction.Service.Helpers;
using EvidentInstruction.Service.Models.Interfaces;
using System;
using System.Collections.Generic;
using EvidentInstruction.Service.Infrastructures;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using EvidentInstruction.Service.Exceptions;

namespace EvidentInstruction.Service.Models
{
    public class WebService : IWebService, IDisposable
    {
        private FlurlProvider fprovider; 

        public WebService(RequestInfo request)
        {
            fprovider = new FlurlProvider(request);
        }

        public ResponceInfo SendMessage(RequestInfo request, Dictionary<HTTPMethodType, HttpMethod> webMethodsDictionary)
        {
            var (isValid, results) = Validate.ValidateModel(request);
            if (isValid)
            {

                try
                {     
                    Log.Logger().LogInformation("Request: " + Environment.NewLine + 
                        request.Url + Environment.NewLine + 
                        request.Method + Environment.NewLine + 
                        request.Content.ReadAsStringAsync().Result);

                    var resp = fprovider.SendRequest(request);
                    var content = ServiceHelpers.GetObjectFromString(resp.Result.Content.ReadAsStringAsync().Result);

                    Log.Logger().LogInformation("Responce: " + Environment.NewLine +
                        resp.Result.StatusCode + Environment.NewLine +
                        resp.Result.Content.ReadAsStringAsync().Result);

                     return new ResponceInfo
                     {
                         Headers = resp.Result.Headers,
                         Content = content,
                         Request = request, 
                         StatusCode = resp.Result.StatusCode
                     };
                    
                }
                catch (FlurlException ex)
                {
                    if (ex.InnerException is FlurlHttpTimeoutException)
                    {
                        Log.Logger().LogError($"Request {request.Url} timed out. {ex}");

                        return new ResponceInfo
                        {
                            Headers = null,
                            Content = null,
                            StatusCode = System.Net.HttpStatusCode.GatewayTimeout,
                            Request = request
                        };
                    }
                    else
                    {
                        if (ex.Call == null)
                        {
                            Log.Logger().LogError(ex.Message);

                            Log.Logger().LogInformation("Responce: " + Environment.NewLine +
                                System.Net.HttpStatusCode.BadRequest);

                            return new ResponceInfo
                            {
                                Headers = null,
                                Content = null,
                                StatusCode = System.Net.HttpStatusCode.BadRequest,
                                Request = request
                            };
                        }
                        else
                        {
                            Log.Logger().LogError(ex.Message);
                            var content = ServiceHelpers.GetObjectFromString(ex.Call.Response.ResponseMessage.Content.ReadAsStringAsync().Result);

                            Log.Logger().LogInformation("Responce: " + Environment.NewLine +
                                ex.Call.Response.StatusCode + Environment.NewLine +
                                ex.Call.Response.ResponseMessage.Content.ReadAsStringAsync().Result);

                            return new ResponceInfo
                            {
                                Headers = ex.Call.Response.ResponseMessage.Headers,
                                Content = content,
                                StatusCode = ex.Call.Response.ResponseMessage.StatusCode,
                                Request = request
                            };
                        }

                    }
                }
                catch (Exception ex)
                {
                    
                    Log.Logger().LogError(ex.Message);
                    Log.Logger().LogInformation("Responce: " + Environment.NewLine +
                               System.Net.HttpStatusCode.BadRequest);

                    return new ResponceInfo
                    {
                        Headers = null,
                        Content = null,
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Request = request
                    };
                }
            }
            else
            {                
                Log.Logger().LogInformation($"Request is not valid "+ Environment.NewLine + 
                        request.Url + Environment.NewLine + 
                        request.Method + Environment.NewLine + 
                        request.Content.ReadAsStringAsync().Result);

                return null;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
