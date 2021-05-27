using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using Molder.Service.Exceptions;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Molder.Service.Models.ClientFactory;
using Molder.Helpers;

namespace Molder.Service.Models.Provider
{
    [ExcludeFromCodeCoverage]
    public class FlurlProvider : IFlurlProvider, IDisposable
    {     
        public async Task<HttpResponseMessage> SendRequestAsync(RequestInfo request)
        { 
            try
            {
                IFlurlResponse responce = null;

                if (request.Credential != null)
                {
                    var credentialClientFactory = new CredentialHttpClientFactory()
                    {
                        CustomCredential = request.Credential
                    };

                    responce = request.Content is null ?
                    await request.Url
                        .WithClient(new FlurlClient(request.Url).Configure(settings => {
                            settings.HttpClientFactory = credentialClientFactory;
                            settings.HttpClientFactory.CreateMessageHandler();
                        }))
                        .WithHeaders(request.Headers)
                        .SendAsync(request.Method) :
                    await request.Url
                        .WithClient(new FlurlClient(request.Url).Configure(settings => {
                            settings.HttpClientFactory = credentialClientFactory;
                            settings.HttpClientFactory.CreateMessageHandler();
                        }))
                        .WithHeaders(request.Headers)
                        .SendAsync(request.Method, request.Content as HttpContent);
                }
                else
                {
                    responce = request.Content is null ?
                    await request.Url
                        .WithHeaders(request.Headers)
                        .SendAsync(request.Method) :
                    await request.Url
                        .WithHeaders(request.Headers)
                        .SendAsync(request.Method, request.Content as HttpContent);
                }

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