using Molder.Helpers;
using Molder.Service.Models;
using System;
using System.Net.Http;

namespace Molder.Service.Helpers
{
    public static class Message
    {
        public static string CreateMessage(this RequestInfo request)
        {
            switch (request.Content)
            {
                case null:
                    return $"Request: {request.Url} {Environment.NewLine} with method {request.Method}";
                default:
                    var requestContentAsString = (request.Content as StringContent).ReadAsStringAsync().GetAwaiter().GetResult();
                    return requestContentAsString.TryParseToXml() ?
                        $"Request: {request.Url} {Environment.NewLine} with method {request.Method} {Environment.NewLine} and content: {Environment.NewLine} {System.Security.SecurityElement.Escape(Reflection.ConvertObject<string>(requestContentAsString.ToXml()))}" :
                        $"Request: {request.Url} {Environment.NewLine} with method {request.Method} {Environment.NewLine} and content: {Environment.NewLine} {requestContentAsString}";
            }
        }

        public static string CreateMessage(this ResponceInfo responce)
        {
            switch (responce.Content)
            {
                case null:
                    return $"Responce: {responce.Request.Url} status: {responce.StatusCode}";
                default:
                    return responce.Content.TryParseToXml() ?
                        $"Responce: {responce.Request.Url} status: {responce.StatusCode} and content: {Environment.NewLine} {System.Security.SecurityElement.Escape(Reflection.ConvertObject<string>(responce.Content.ToXml()))}" :
                        $"Responce: {responce.Request.Url} status: {responce.StatusCode} and content: {Environment.NewLine} {responce.Content}"; ;
            }
        }
    }
}
