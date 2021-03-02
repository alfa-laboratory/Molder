using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using Molder.Helpers;
using Molder.Service.Exceptions;
using Flurl.Http;
using Microsoft.Extensions.Logging;

namespace Molder.Service.Models.Provider
{
    [ExcludeFromCodeCoverage]
    public class FlurlProvider : IFlurlProvider, IDisposable
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
                throw new FlurlException(ex);
            }
            catch (FlurlHttpException ex)
            {
                throw new FlurlException(ex);
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    } 
}