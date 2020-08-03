using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using EvidentInstruction.Exceptions;
using EvidentInstruction.Helpers;
using Flurl;
using Flurl.Http;

namespace EvidentInstruction.Service.Models
{
    public static class FlurlExtensions
    {
        public static async Task<HttpResponseMessage> FlSend(this IFlurlRequest ur, string url, HttpMethod method, HttpContent content)
        {
            try
            {
                return url.SendAsync(method, content).Result;
            }
            catch (Exception e)
            {
                if (e.InnerException is FlurlHttpException ex)
                {
                    throw new ServiceException(call:new HttpCall(), "Произошла ошибка flurl сервиса",ex);
                }
                else throw new ServiceTimeoutException(call: new HttpCall(), "Время запроса истекло ", e);
            }
        }

        public static T FlHeaders<T>(this T ur, object headers) where T : IHttpSettingsContainer
        {
            try
            {
                return ur.WithHeaders<T>(headers);
            }
            catch (WithHeadersException e)
            {
                Log.Logger.Warning($" \"{e.Message}\" ");
                throw new WithHeadersException(e.Message, e);
            }
        }

        public static IFlurlRequest FlTimeout(this string url, int seconds)
        {
            try
            {
                return new FlurlRequest(url).WithTimeout(seconds);
            }
            catch (WithTimeoutException e)
            {
                Log.Logger.Warning($" \"{e.Message}\" ");
                throw new WithTimeoutException(e.Message, e);
            }
        }

        public static Url FlSetParams(this string url, IEnumerable<string> names)
        {
            try
            {
                return new Url(url).SetQueryParams(names);
            }
            catch (ArgumentNullException e)
            {
                Log.Logger.Warning($" \"{e.Message}\" ");
                throw new ArgumentNullException(e.Message, e);
            }
        }
    }
}
