using Flurl.Http;
using Molder.Helpers;
using Molder.Service.Helpers;
using Molder.Service.Models.Interfaces;
using System;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Molder.Service.Models
{
    public class WebService : IWebService, IDisposable
    {
        public IFlurlProvider fprovider; 

        public WebService(RequestInfo request)
        {
            fprovider = new FlurlProvider(request);
        }

        public ResponceInfo SendMessage(RequestInfo request)
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
                catch (FlurlHttpTimeoutException)
                {
                    Log.Logger().LogError("Request:" + request.Url + Environment.NewLine +
                                          request.Method + Environment.NewLine +
                                          request.Content.Headers.ToString() + Environment.NewLine + 
                                          "timed out."); 

                    return new ResponceInfo
                    {
                        Headers = null,
                        Content = new StringContent(string.Empty),
                        StatusCode = System.Net.HttpStatusCode.GatewayTimeout,
                        Request = request
                    };
                }
                catch (Exception ex)
                {
                    if (ex.InnerException is FlurlHttpException fex)
                    {
                        if (fex.Call == null)
                        {
                            Log.Logger().LogError("Request:" + request.Url + Environment.NewLine +
                                                  request.Method + Environment.NewLine +
                                                  request.Content.Headers.ToString() + Environment.NewLine + 
                                                  "failed:" + fex.Message);

                            Log.Logger().LogInformation("Responce status code: " + Environment.NewLine +
                                System.Net.HttpStatusCode.BadRequest);

                            return new ResponceInfo
                            {
                                Headers = null,
                                Content = new StringContent(string.Empty),
                                StatusCode = System.Net.HttpStatusCode.BadRequest,
                                Request = request
                            };
                        }
                        else
                        {
                            string fexResult = string.Empty;

                            if(fex.Call.Response != null)
                            {
                                fexResult = fex.Call.Response.ResponseMessage.Content.ReadAsStringAsync().Result;

                                Log.Logger().LogInformation("Responce: " + Environment.NewLine +
                                    fex.Call.Response.StatusCode + Environment.NewLine +
                                     fexResult);
                            }
                            else
                            {
                                Log.Logger().LogInformation("Responce is empty. Check request parameters.");

                                Log.Logger().LogError("Request:" + request.Url + Environment.NewLine +
                                                  request.Method + Environment.NewLine +
                                                  request.Content.Headers.ToString() + Environment.NewLine +
                                                  "failed:" + fex.Message);

                                return null;
                            }

                            Log.Logger().LogError("Request:" + request.Url + Environment.NewLine +
                                                  request.Method + Environment.NewLine +
                                                  request.Content.Headers.ToString() + Environment.NewLine +
                                                  "failed:" + fex.Message);
                           

                              var content = ServiceHelpers.GetObjectFromString(fexResult);  

                              return new ResponceInfo
                              {
                                    Headers = fex.Call.Response.ResponseMessage.Headers,
                                    Content = content,
                                    StatusCode = fex.Call.Response.ResponseMessage.StatusCode,
                                    Request = request
                              };
                        }
                    }
                    Log.Logger().LogError("Request:" + request.Url + Environment.NewLine +
                                          request.Method + Environment.NewLine +
                                          request.Content.Headers.ToString() + Environment.NewLine +
                                          "failed:" + ex.Message);

                    Log.Logger().LogInformation("Responce: " + Environment.NewLine +
                               System.Net.HttpStatusCode.BadRequest);

                    return new ResponceInfo
                    {
                        Headers = null,
                        Content = new StringContent(string.Empty),
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
