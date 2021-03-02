using Molder.Helpers;
using Molder.Service.Exceptions;
using Molder.Service.Models;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using System;

namespace Molder.Service.Helpers
{
    public static class FlurlExceptionsHelper
    {
        public static ResponceInfo GetResponce(this Exception ex, RequestInfo request)
        {
            if (ex is FlurlException)
            {
                if (((FlurlException)ex).ExceptionName == (typeof(FlurlHttpTimeoutException)).Name)
                {
                    return new ResponceInfo
                    {
                        Headers = null,
                        Content = null,
                        StatusCode = System.Net.HttpStatusCode.GatewayTimeout,
                        Request = request
                    };
                }

                if (((FlurlException)ex).ExceptionName == (typeof(FlurlHttpException)).Name)
                {
                    var exception = (((FlurlException)ex).Exception as FlurlHttpException)?.Call.Response;
                    var content = exception?.ResponseMessage.Content.ReadAsStringAsync().Result;

                    var statusCode = exception == null ?
                                        System.Net.HttpStatusCode.BadRequest :
                                        exception.ResponseMessage.StatusCode;

                    var responce = new ResponceInfo
                    {
                        Headers = null,
                        Content = content,
                        StatusCode = statusCode,
                        Request = request
                    };

                    Log.Logger().LogInformation($"{Message.CreateMessage(responce)}. {ex}");
                    return responce;
                }
            }

            return new ResponceInfo
            {
                Headers = null,
                Content = null,
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Request = request
            };
        }
    }
}