using Newtonsoft.Json.Linq;
using EvidentInstruction.Helpers;
using EvidentInstruction.Service.Helpers;
using EvidentInstruction.Service.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace EvidentInstruction.Service.Models
{
    public class WebService : IWebService, IDisposable
    {

        public static IContent JsonContent = new JsonContent();
        public static IContent XmlContent = new XmlContent();
        public static IContent TextContent = new TextContent();


        Interfaces.IServiceProvider serviceProvider = new ServiceProvider();
        public ResponseInfo SendMessage(RequestInfo request)
        {
            var (isValid, results) = Validate.ValidateModel(request);
            if (isValid)
            {
                var result = serviceProvider.WrapMethod(request);
                if (result.Item1)
                {
                    var content =
                        ServiceHelpers.GetObjectFromString(serviceProvider.response.Result.Content.ReadAsStringAsync()
                            .Result);
                    Log.Logger.Information("Response: " + Environment.NewLine +
                                           serviceProvider.response.Result.StatusCode + Environment.NewLine +
                                           serviceProvider.response.Result.Content.ReadAsStringAsync().Result);
                    return result.Item2;

                }

                return result.Item2;
            }
            else
            {

                Log.Logger.Information("Response: " + Environment.NewLine +
                                System.Net.HttpStatusCode.BadRequest);
                return null;
            }
        }

        public static Dictionary<string, string> ReplaceHeaders(Dictionary<string, string> headers, string str)
        {
            var nHeaders = new Dictionary<string, string>();
            var contentType = string.Empty;
            var doc = ServiceHelpers.GetObjectFromString(str);

            switch (doc)
            {
                case XmlDocument xmlDoc:
                case XDocument xDoc:
                    {
                        contentType = XmlContent.Get(doc);
                        break;
                    }
                case JObject jObject:
                    {
                        contentType = JsonContent.Get(doc);
                        break;
                    }
                default:
                    {
                        contentType = TextContent.Get(doc);
                        break;
                    }
            }

            if (!headers.ContainsKey("Content-Type"))
            {
                nHeaders = headers;
                nHeaders.Add("Content-Type", contentType);
            }
            return headers;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
