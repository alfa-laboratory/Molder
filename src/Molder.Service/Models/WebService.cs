using Molder.Helpers;
using Molder.Service.Helpers;
using Molder.Service.Models.Interfaces;
using System;
using Microsoft.Extensions.Logging;
using Molder.Service.Exceptions;
using System.Threading.Tasks;
using Molder.Service.Models.Provider;
using System.Threading;

namespace Molder.Service.Models
{
    public class WebService : IWebService, IDisposable
    {
        protected AsyncLocal<IFlurlProvider> flurlProvider = new AsyncLocal<IFlurlProvider> { Value = null };
        public IFlurlProvider Provider
        {
            get => flurlProvider.Value;
            set
            {
                flurlProvider.Value = value;
            }
        }

        public WebService()
        {
            Provider = new FlurlProvider();
        }

        public async Task<ResponceInfo> SendMessage(RequestInfo request)
        {
            var isValid = Helpers.Validate.ValidateUrl(request.Url);
            if (isValid)
            {
                try
                {
                    Log.Logger().LogInformation(Helpers.Message.CreateMessage(request));

                    var responce = await Provider.SendRequestAsync(request);
                            
                    var responceInfo =  new ResponceInfo
                    {
                        Headers = responce.Headers,
                        Content = responce.Content?.ReadAsStringAsync().Result,
                        Request = request,
                        StatusCode = responce.StatusCode
                    };

                    Log.Logger().LogInformation(Helpers.Message.CreateMessage(responceInfo));

                    return responceInfo;
                }
                catch (FlurlException ex)
                {
                    return ex.GetResponce(request);
                }
                catch (Exception ex) 
                {
                    return ex.GetResponce(request);
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
