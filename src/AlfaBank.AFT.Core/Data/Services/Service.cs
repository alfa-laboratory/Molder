using AlfaBank.AFT.Core.Exceptions;
using AlfaBank.AFT.Core.Infrastructure.Service;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace AlfaBank.AFT.Core.Data.Services
{
    public abstract class Service : IService, ICloneable
    {
        public string Url { get; set; }
        public WebServiceMethod WebServiceMethod { get; set; }
        public NameValueCollection Headers { get; set; }
        public string UserAgent { get; set; }
        public IWebProxy Proxy { get; set; } = null;
        public int ConnectionLimit { get; set; } = 20;
        public int Timeout { get; set; } = 60000;
        public ICredentials Credentials { get; set; } = null;
        protected object Data { get; set; }

        public void Dispose()
        {
            Data = null;
        }

        public abstract (HttpStatusCode?, List<Error>) CallWebService(string body = null);

        /// <inheritdoc />
        public override string ToString()
        {
            return Encoding.UTF8.GetString((byte[])Data);
        }

        public object ToObject()
        {
            return Data;
        }

        public virtual JToken ToJson()
        {
            var str = Encoding.UTF8.GetString((byte[])Data);
            return JToken.Parse(str);
        }

        public XDocument ToXml()
        {
            var str = Encoding.UTF8.GetString((byte[])Data);
            var xmlDoc = XDocument.Parse(str);
            return xmlDoc;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        protected (HttpWebRequest, List<Error>) CreateRequest()
        {
            var errors = new List<Error>();
            try
            {
               var request = (HttpWebRequest)WebRequest.Create(Url);
               if(!string.IsNullOrWhiteSpace(UserAgent))
               {
                   request.UserAgent = UserAgent;
               }

               request.Timeout = Math.Max(0, Timeout);
               request.Proxy = Proxy;
               request.ServicePoint.ConnectionLimit = ConnectionLimit;
               request.Credentials = Credentials;
               request.PreAuthenticate = true;
               if(Headers != null)
               {
                   request.Headers.Add(Headers);
               }

               request.Method = WebServiceMethod.ToString();

               return (request, errors);
            }
            catch (Exception e)
            {
                errors.Add(new Error
                {
                    TargeBase = e.TargetSite,
                    Message = e.Message,
                    Type = e.GetType()
                });
                return (null, errors);
            }
        }

        protected (HttpStatusCode?, List<Error>) SendHttpRequest(string data = null)
        {
            HttpStatusCode? statusCode = null;
            var listErrors = new List<Error>();
            try
            {
                var (request, errors) = CreateRequest();
                if(errors.Count == 0)
                {
                    if(string.IsNullOrWhiteSpace(data))
                    {
                        request.ContentLength = 0;
                    }
                    else
                    {
                        var dataBytes = Encoding.UTF8.GetBytes(data);
                        request.ContentLength = dataBytes.Length;
                        using(var stream = request.GetRequestStream())
                        {
                            stream.Write(dataBytes, 0, dataBytes.Length);
                        }
                    }

                    var response = (HttpWebResponse)request.GetResponse();
                    statusCode = response.StatusCode;

                    using(var stream = response.GetResponseStream())
                    {
                        using(var res = new MemoryStream())
                        {
                            stream?.CopyTo(res);
                            Data = res.ToArray();
                        }
                    }

                    response.Dispose();
                }
                else
                {
                    listErrors = errors;
                }
            }
            catch (Exception e)
            {
                Data = null;
                listErrors.Add(new Error
                {
                    TargeBase = e.TargetSite,
                    Message = e.Message,
                    Type = e.GetType()
                });
                return (null, listErrors);
            }

            return (statusCode, listErrors);
        }
    }
}
