using Molder.Service.Models;
using System;
using System.Net.Http;

namespace Molder.Service.Helpers
{
    public static class Message
    {
        public static string CreateMessage(this RequestInfo request)
        {
            return request.Content == null ?
                    $"Request: {request.Url} {Environment.NewLine} with method {request.Method}" :
                    $"Request: {request.Url} {Environment.NewLine} with method {request.Method} {Environment.NewLine} and content: {(request.Content as StringContent).ReadAsStringAsync().GetAwaiter().GetResult()}";              
       
        }

        public static string CreateMessage(this ResponceInfo responce)
        {            
            return responce.Content == null ?
                     $"Responce: {responce.StatusCode}" :
                     $"Responce: {responce.StatusCode} {Environment.NewLine} {responce.Content}";
        }
    }
}
