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
        public static ResponceInfo GetResponceFromException(Exception ex, RequestInfo request)
        {
            if (((FlurlException)ex).ExceptionName == (typeof(FlurlHttpTimeoutException)).Name)
            {
                Log.Logger().LogError($"{Messages.CreateMessage(request)} timed out. {ex}");

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
                var exception = (ex as FlurlHttpException)?.Call.Response;

                Log.Logger().LogError($"{Messages.CreateMessage(request)} is failed.");

                var content = exception == null ?
                                    null :
                                    exception.ResponseMessage.Content.ReadAsStringAsync().Result;

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

                Log.Logger().LogInformation($"{Messages.CreateMessage(responce)}. {ex}");

                return responce;
            }

            Log.Logger().LogError($"{Messages.CreateMessage(request)} is failed. {ex}");

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