using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EvidentInstruction.Helpers;
using EvidentInstruction.Service.Helpers;
using EvidentInstruction.Service.Infrastructures;
using EvidentInstruction.Service.Models.Interfaces;
using Flurl.Http;
using Newtonsoft.Json.Linq;

namespace EvidentInstruction.Service.Models
{  
    public class FlurlProvider/* :  IDisposable*/
    {
        private readonly string url;
        private readonly HttpContent content;

        public FlurlProvider(RequestInfo request)
        {
            url = request.Url;
            content = request.Content;
        }

        //в конструктор пропихнуть урл, метож, header, 
        //Http content
        //recuestinfo в него попадает url, метод, тело, методы
        public Task<HttpResponseMessage> NewSend(RequestInfo request, Dictionary<string,string> dicc)
        {

            //Get есть метод (тип: параметр)
            try
            {

                var key = dicc.Select(x => x.Key).Where(y => y.ToLower().Contains("auth")).First();

                var login = dicc[key].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                var responce = url
                    .WithBasicAuth(login[0], login[1])
                    .WithHeaders(dicc)
                    .SendAsync(request.Method, content);

                return responce;
            }
            catch (FlurlHttpTimeoutException)
            {
                //свой эксепшен

                throw new Exception();
            }

            catch (Exception)
            {
                //свой эксепшен если не заполняет словарь

                throw new Exception();
            }

        }

       


/*
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }*/
    }     
   
}
