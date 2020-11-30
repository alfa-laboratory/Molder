using Flurl.Http;
using EvidentInstruction.Helpers;
using EvidentInstruction.Service.Helpers;
using EvidentInstruction.Service.Models.Interfaces;
using System;
using System.Collections.Generic;
using EvidentInstruction.Service.Infrastructures;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace EvidentInstruction.Service.Models
{
    public class WebService : IWebService, IDisposable
    {
        public ResponceInfo SendMessage(RequestInfo request, Dictionary<HTTPMethodType, HttpMethod> webMethodsDictionary)
        {
            var (isValid, results) = Validate.ValidateModel(request);
            if (isValid)
            {
               var headers = ServiceHelpers.ReplaceHeaders(request.Headers, request);           

                try
                {                  

                    Log.Logger().LogInformation("Request: " + Environment.NewLine + 
                        request.Url + Environment.NewLine + 
                        request.Method + Environment.NewLine + 
                        request.Content.ReadAsStringAsync().Result);

                    //FlurlProvider
                    FlurlProvider fprovider= new FlurlProvider(request);

                    var resp = fprovider.SendRequest(request, request.Headers);

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
                catch (FlurlHttpTimeoutException)
                {
                    Log.Logger().LogError("Request timed out.");

                    return new ResponceInfo
                    {
                        Headers = null,
                        Content = null,
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Request = request
                    };
                }
                catch (Exception ex)
                {
                    if (ex.InnerException is FlurlHttpException fex)
                    {
                        if (fex.Call == null)
                        {
                            Log.Logger().LogError(fex.Message);

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
                            Log.Logger().LogError(fex.Message);
                             var content = ServiceHelpers.GetObjectFromString(fex.Call.Response.ResponseMessage.Content.ReadAsStringAsync().Result);

                             Log.Logger().LogInformation("Responce: " + Environment.NewLine +
                                 fex.Call.Response.StatusCode + Environment.NewLine +
                                 fex.Call.Response.ResponseMessage.Content.ReadAsStringAsync().Result);

                            return new ResponceInfo
                            {
                                Headers = fex.Call.Response.ResponseMessage.Headers,
                                Content = content,
                                StatusCode = fex.Call.Response.ResponseMessage.StatusCode,
                                Request = request
                            };
                        }
                    }
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
                Log.Logger().LogInformation("Responce: " + Environment.NewLine +
                                System.Net.HttpStatusCode.BadRequest);
                return null;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
