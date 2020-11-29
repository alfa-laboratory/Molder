using Flurl.Http;
using Newtonsoft.Json.Linq;
using EvidentInstruction.Helpers;
using EvidentInstruction.Service.Helpers;
using EvidentInstruction.Service.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using EvidentInstruction.Service.Infrastructures;
using System.Linq;
using System.Net.Http;

namespace EvidentInstruction.Service.Models
{
    public class WebService : IWebService, IDisposable
    {
        public ResponceInfo SendMessage(RequestInfo request, Dictionary<HTTPMethodType, HttpMethod> webMethodsDictionary)
        {
            var (isValid, results) = Validate.ValidateModel(request);
            if (isValid)
            {
                var headers = ServiceHelpers.ReplaceHeaders(request.Headers, request.Content.ReadAsStringAsync().Result, request); //возвращать кортеж ?            

                try
                {
                    
                    Log.Logger.Information("Request: " + Environment.NewLine + 
                        request.Url + Environment.NewLine + 
                        request.Method + Environment.NewLine + 
                        request.Content.ReadAsStringAsync().Result);
                    //FlurlProvider

                    FlurlProvider dd = new FlurlProvider(request);

                    var resp = dd.NewSend(request, headers); 

                    var content = ServiceHelpers.GetObjectFromString(resp.Result.Content.ReadAsStringAsync().Result);

                    Log.Logger.Information("Responce: " + Environment.NewLine +
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
                catch (FlurlHttpTimeoutException) //тут свой эксепшен
                {
                    Log.Logger.Error("Request timed out.");
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
                        if (fex.Call.HttpStatus == null)
                        {
                            Log.Logger.Error(fex.Message);

                            Log.Logger.Information("Responce: " + Environment.NewLine +
                                System.Net.HttpStatusCode.BadRequest);

                            return new ResponceInfo
                            {
                                Headers = null,
                                Content = null,
                                StatusCode = System.Net.HttpStatusCode.BadRequest,
                                Request = request
                            };
                        }else
                        {
                            Log.Logger.Error(fex.Message);
                            var content = ServiceHelpers.GetObjectFromString(fex.Call.Response.Content.ReadAsStringAsync().Result);

                            Log.Logger.Information("Responce: " + Environment.NewLine +
                                fex.Call.Response.StatusCode + Environment.NewLine +
                                fex.Call.Response.Content.ReadAsStringAsync().Result);

                            return new ResponceInfo
                            {
                                Headers = fex.Call.Response.Headers,
                                Content = content,
                                StatusCode = fex.Call.Response.StatusCode,
                                Request = request
                            };
                        }
                    }
                    Log.Logger.Error(ex.Message);
                    Log.Logger.Information("Responce: " + Environment.NewLine +
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
                
                Log.Logger.Information("Responce: " + Environment.NewLine +
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
