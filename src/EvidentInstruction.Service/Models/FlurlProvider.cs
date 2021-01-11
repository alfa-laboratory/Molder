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
        private readonly string url;
        private readonly HttpContent content;

        public FlurlProvider(RequestInfo request)
        {
            url = request.Url;
            content = request.Content;
        }
        
        public async Task<HttpResponseMessage> SendRequest(RequestInfo request)
        {
            IFlurlResponse responce;

            try
            {
                if(request.Content.Headers.ContentLength == 0)
                {
                    responce = await url                                    
                                    .WithHeaders(request.Headers)
                                    .SendAsync(request.Method);
                }
                else
                {
                    responce = await url                                   
                                    .WithHeaders(request.Headers)
                                    .SendAsync(request.Method, content);
                }
               
                return responce.ResponseMessage;                
            }
            catch (FlurlHttpTimeoutException ex)
            {
                Log.Logger().LogWarning($"Request {request.Url} timed out. {ex}");
                throw new FlurlException(ex.Call, ex);
                
            }
            catch (FlurlHttpException ex)
            {
                Log.Logger().LogWarning($"Request {request.Url} failed. {ex}");
                throw new FlurlException(ex.Call, ex);
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    } 
}
