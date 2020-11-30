using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http;

namespace EvidentInstruction.Service.Models
{  
    public class FlurlProvider :  IDisposable
    {
        private readonly string url;
        private readonly HttpContent content;

        public FlurlProvider(RequestInfo request)
        {
            url = request.Url;
            content = request.Content;
        }
        
        public async Task<HttpResponseMessage> SendRequest(RequestInfo request, Dictionary<string,string> dicc)
        {

            try
            {

               // var key = dicc.Select(x => x.Key).Where(y => y.ToLower().Contains("auth")).First();
               // var login = dicc[key].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                var responce = await url
                  //  .WithBasicAuth(login[0], login[1])
                    .WithHeaders(dicc)
                    .SendAsync(request.Method, content);

                return responce.ResponseMessage;                
            }
            catch (FlurlHttpTimeoutException)
            {
                //TODO свой Exception

                throw new Exception();
            }

            catch (Exception)
            {
                //TODO свой Exception

                throw new Exception();
            }

        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }     
   
}
