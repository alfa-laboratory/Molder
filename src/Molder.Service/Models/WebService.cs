using Molder.Helpers;
using Molder.Service.Helpers;
using Molder.Service.Models.Interfaces;
using System;
using Microsoft.Extensions.Logging;
using Molder.Service.Exceptions;
using System.Threading.Tasks;

namespace Molder.Service.Models
{
    public class WebService : IWebService, IDisposable
    {
        public IFlurlProvider fprovider;
        private RequestInfo request;

        public WebService(RequestInfo request)
        {
            fprovider = new FlurlProvider();
            this.request = request;
        }

        public async Task<ResponceInfo> SendMessage()
        {
            var isValid = Validate.ValidateUrl(request.Url);

            if (isValid)
            {
                try
                {
                    Log.Logger().LogInformation($"{Messages.CreateMessage(request)}");

                    var resp = await fprovider.SendRequestAsync(request);

                    var content = ServiceHelpers.GetObjectFromString(resp.Content?.ReadAsStringAsync().Result); 
                            
                    var responce =  new ResponceInfo
                    {
                        Headers = resp.Headers,
                        Content = content,
                        Request = request,
                        StatusCode = resp.StatusCode
                    };

                    Log.Logger().LogInformation($"{ Messages.CreateMessage(responce)}");

                    return responce;
                }
                catch (FlurlException ex)
                {
                    return FlurlExceptionsHelper.GetResponceFromException(ex, request);
                }
                catch (Exception ex) 
                {
                    return FlurlExceptionsHelper.GetResponceFromException(ex, request);    
                }                
            }

            Log.Logger().LogError($"Url:{ request.Url} is not valid." );

            return null;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
