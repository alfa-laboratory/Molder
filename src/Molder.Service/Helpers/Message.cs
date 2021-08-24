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
                    var requestContentAsString = (request.Content as StringContent)?.ReadAsStringAsync().GetAwaiter().GetResult();
                    return requestContentAsString.TryParseToXml() ?
                        $"Request: {request.Url} {Environment.NewLine} with method {request.Method}, Timeout {request.Timeout} {Environment.NewLine} and content: {Environment.NewLine} {Converter.CreateXMLEscapedString(requestContentAsString.ToXml())}" :
                        $"Request: {request.Url} {Environment.NewLine} with method {request.Method}, Timeout {request.Timeout} {Environment.NewLine} and content: {Environment.NewLine} {requestContentAsString}";
            }
        }

        public static string CreateMessage(this ResponceInfo responce)
        {
            return responce.Content switch
            {
                null => $"Responce: {responce.Request.Url} status: {responce.StatusCode}",
                _ => responce.Content.TryParseToXml()
                    ? $"Responce: {responce.Request.Url} status: {responce.StatusCode} and content: {Environment.NewLine} {Converter.CreateXMLEscapedString(responce.Content.ToXml())}"
                    : $"Responce: {responce.Request.Url} status: {responce.StatusCode} and content: {Environment.NewLine} {responce.Content}"
            };
        }
    }
}
