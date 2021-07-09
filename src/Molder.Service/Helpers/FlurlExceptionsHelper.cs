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
            if (!(ex is FlurlException)) return null;
            switch (((FlurlException)ex).ExceptionName)
            {
                case nameof(FlurlHttpTimeoutException):
                    return new ResponceInfo
                    {
                        Headers = null,
                        Content = null,
                        StatusCode = System.Net.HttpStatusCode.GatewayTimeout,
                        Request = request
                    };
                case nameof(FlurlHttpException):
                {
                    var exception = (((FlurlException)ex).Exception as FlurlHttpException)?.Call.Response;
                    var content = exception?.ResponseMessage.Content.ReadAsStringAsync().Result;

                    var responce = new ResponceInfo
                    {
                        Headers = null,
                        Content = content,
                        StatusCode = exception.ResponseMessage.StatusCode,
                        Request = request
                    };

                    Log.Logger().LogInformation($"{responce.CreateMessage()}. \n\nInner exception: {ex}");
                    return responce;
                }
                default:
                    return null;
            }
        }
    }
}