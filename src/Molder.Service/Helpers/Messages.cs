using Molder.Service.Models;
using System;

namespace Molder.Service.Helpers
{
    public static class Messages
    {
        public static string CreateMessage(RequestInfo request)
        {
            return request.Content == null ?
                    $"Request: {request.Url} {Environment.NewLine} with method {request.Method}" :
                    $"Request: {request.Url} {Environment.NewLine} with method {request.Method} {Environment.NewLine} and content: {request.Content.ReadAsStringAsync().Result}";              
       
        }

        public static string CreateMessage(ResponceInfo responce)
        {            
            return responce.Content == null ?
                     $"Responce: {responce.StatusCode}" :
                     $"Responce: {responce.StatusCode} {Environment.NewLine} {responce.Content}";
        
        }
    }
}
