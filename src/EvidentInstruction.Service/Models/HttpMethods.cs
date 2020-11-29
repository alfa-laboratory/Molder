using EvidentInstruction.Service.Infrastructures;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EvidentInstruction.Service.Models
{    
    public abstract class HttpMethods
    {
        public abstract HTTPMethodType Type { get; }

        public abstract Task<HttpResponseMessage> SendMess(Dictionary<string, string> headers, string url, HttpContent content = null);
    }

    public class MethodGet : HttpMethods
    {
        public override HTTPMethodType Type { get => HTTPMethodType.GET; }      

        public override async Task<HttpResponseMessage> SendMess(Dictionary<string, string> headers, string url, HttpContent content = null)
        {
            var res = await url
                .WithHeaders(headers)                
                .GetAsync();

            return res;
                
        }
    }

    /*public class MethodPut : HttpMethods
    {
        public override HTTPMethodType Type { get => HTTPMethodType.PUT; }

        public override async Task<HttpResponseMessage> SendMess(RequestInfo requestInfo) //header/ uri/content content по умолчанию null
        {
            //тут пропервка, что contet
           var res =  await requestInfo.Url
                .WithHeader("","")
                .PutStringAsync(requestInfo.Content.ToString());

            return res;
        }
    }*/

    /*public class MethodPost : HttpMethods
    {
        public override HTTPMethodType type { get => HTTPMethodType.POST; }

        public override async Task SendMess(string url, HTTPMethodType type)
        {
            await url.GetAsync();
        }
    }

    public class MethodDelete : HttpMethods
    {
        public override HTTPMethodType type { get => HTTPMethodType.DELETE; }

        public override async Task SendMess(string url, HTTPMethodType type)
        {
            await url.GetAsync();
        }
    }*/
}
