using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using EvidentInstruction.Helpers;
using EvidentInstruction.Service.Exceptions;
using EvidentInstruction.Service.Models.Interfaces;
using Flurl.Http;
using Microsoft.Extensions.Logging;

namespace EvidentInstruction.Service.Models
{
    [ExcludeFromCodeCoverage]
    public class FlurlProvider : IFlurlProvider,  IDisposable
    {     
        public async Task<HttpResponseMessage> SendRequestAsync(RequestInfo request)
        { 
            try
            {
                var responce = request.Content is null ? 
                           await request.Url
                               .WithHeaders(request.Headers)                              
                               .SendAsync(request.Method) : 
                           await request.Url
                               .WithHeaders(request.Headers)
                               .SendAsync(request.Method, (request.Content as HttpContent));
               
                return responce.ResponseMessage;                
            }
            catch (FlurlHttpTimeoutException ex)
            {
                Log.Logger().LogWarning($"Request {request.Url} timed out. {ex}");                
                throw new FlurlException($"Request {request.Url} timed out. {ex}", ex);
            }
            catch (FlurlHttpException ex)
            {
                Log.Logger().LogWarning($"Request {request.Url} failed. {ex}");  
                throw new FlurlException($"Request {request.Url} failed. {ex}", ex);
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    } 
}
