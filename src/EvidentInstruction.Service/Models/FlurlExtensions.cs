using System;
using System.Net.Http;
using System.Threading.Tasks;
using EvidentInstruction.Exceptions;
using EvidentInstruction.Helpers;
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
                    throw new ServiceException("Произошла ошибка flurl сервиса");
                }
                else throw new ServiceTimeoutException("Время запроса истекло ", e);
            }
        }

        public static T FlHeaders<T>(this T ur, object headers, string url) where T : IHttpSettingsContainer
        {
            try
            {
                return ur.WithHeaders<T>(headers);
            }
            catch (WithHeadersException e)
            {
                Log.Logger.Warning($" \"{e.Message}\" ");
                throw new WithHeadersException(e.Message);
            }
        }

        public static IFlurlRequest FlTimeout(this string ur, string url, int seconds)
        {
            try
            {
                return new FlurlRequest(url).WithTimeout(seconds);
            }
            catch (WithTimeoutException e)
            {
                Log.Logger.Warning($" \"{e.Message}\" ");
                throw new WithTimeoutException(e.Message);
            }
        }
    }
}
