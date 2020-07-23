using System;
using System.Net.Http;
using System.Threading.Tasks;
using EvidentInstruction.Helpers;
using EvidentInstruction.Service.Helpers;
using Flurl.Http;
using IServiceProvider = EvidentInstruction.Service.Models.Interfaces.IServiceProvider;

namespace EvidentInstruction.Service.Models
{
    public class ServiceProvider : IServiceProvider
    {
        public Task<HttpResponseMessage> response { get; set; }

        public (bool, ResponseInfo) WrapMethod(RequestInfo request)
        {
            var headers = WebService.ReplaceHeaders(request.Headers, request.Content.ReadAsStringAsync().Result);
            try
            {
                Log.Logger.Information("Request: " + Environment.NewLine +
                                       request.Url + Environment.NewLine +
                                       request.Method + Environment.NewLine +
                                       request.Content.ReadAsStringAsync().Result);

                response = request.Url
                    .WithHeaders(headers)
                    .SendAsync(request.Method, request.Content);
                var responseInfo = new ResponseInfo()
                {
                    Headers = response.Result.Headers,
                    Content = ServiceHelpers.GetObjectFromString(response.Result.Content.ReadAsStringAsync().Result),
                    Request = request,
                    StatusCode = response.Result.StatusCode
                };
                return (true, responseInfo);
            }
            catch (FlurlHttpException e)
            {
                Log.Logger.Warning("Request timed out.");
                var responseInfo = new ResponseInfo()
                {
                    Headers = null,
                    Content = null,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Request = request
                };
                return (false, responseInfo);
            }
            catch (Exception ex)
            {
                if (ex.InnerException is FlurlHttpException fex)
                {
                    if (fex.Call.HttpStatus == null)
                    {
                        Log.Logger.Error(fex.Message);

                        Log.Logger.Information("Response: " + Environment.NewLine +
                            System.Net.HttpStatusCode.BadRequest);
                        var responseInfo1 = new ResponseInfo
                        {
                            Headers = null,
                            Content = null,
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Request = request
                        };
                        return (false, responseInfo1);
                    }
                    else
                    {
                        Log.Logger.Error(fex.Message);
                        var content = ServiceHelpers.GetObjectFromString(fex.Call.Response.Content.ReadAsStringAsync().Result);

                        Log.Logger.Information("Response: " + Environment.NewLine +
                            fex.Call.Response.StatusCode + Environment.NewLine +
                            fex.Call.Response.Content.ReadAsStringAsync().Result);

                        var responseInfo2 = new ResponseInfo
                        {
                            Headers = fex.Call.Response.Headers,
                            Content = content,
                            StatusCode = fex.Call.Response.StatusCode,
                            Request = request
                        };
                        return (false, responseInfo2);
                    }
                }
                Log.Logger.Error(ex.Message);
                Log.Logger.Information("Response: " + Environment.NewLine +
                            System.Net.HttpStatusCode.BadRequest);
                var responseInfo = new ResponseInfo
                {
                    Headers = null,
                    Content = null,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Request = request
                };
                return (false, responseInfo);
            }

        }
    }
}
