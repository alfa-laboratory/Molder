using Flurl.Http;
using Newtonsoft.Json.Linq;
using EvidentInstruction.Helpers;
using EvidentInstruction.Service.Helpers;
using EvidentInstruction.Service.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace EvidentInstruction.Service.Models
{
    public class WebService : IWebService, IDisposable
    {
        public ResponceInfo SendMessage(RequestInfo request)
        {
            var (isValid, results) = Validate.ValidateModel(request);
            if (isValid)
            {
                var headers = ReplaceHeaders(request.Headers, request.Content.ReadAsStringAsync().Result);
                try
                {
                    Log.Logger.Information("Request: " + Environment.NewLine + 
                        request.Url + Environment.NewLine + 
                        request.Method + Environment.NewLine + 
                        request.Content.ReadAsStringAsync().Result);
                    
                    var responce = request.Url
                        .WithHeaders(headers)
                        .SendAsync(request.Method, request.Content);
                    var content = ServiceHelpers.GetObjectFromString(responce.Result.Content.ReadAsStringAsync().Result);

                    Log.Logger.Information("Responce: " + Environment.NewLine +
                        responce.Result.StatusCode + Environment.NewLine +
                        responce.Result.Content.ReadAsStringAsync().Result);

                    return new ResponceInfo
                    {
                        Headers = responce.Result.Headers,
                        Content = content,
                        Request = request,
                        StatusCode = responce.Result.StatusCode
                    };
                }
                catch (FlurlHttpTimeoutException)
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

        private Dictionary<string, string> ReplaceHeaders(Dictionary<string, string> headers, string str)
        {
            var nHeaders = new Dictionary<string, string>();
            var contentType = string.Empty;
            var doc = ServiceHelpers.GetObjectFromString(str);

            switch (doc)
            {
                case XmlDocument xmlDoc:
                case XDocument xDoc:
                    {
                        contentType = "text/xml";
                        break;
                    }
                case JObject jObject:
                    {
                        contentType = "application/json";
                        break;
                    }
                default:
                    {
                        contentType = "text/plain";
                        break;
                    }
            }

            if (!headers.ContainsKey("Content-Type"))
            {
                nHeaders = headers;
                nHeaders.Add("Content-Type", contentType);
            }
            return headers;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
