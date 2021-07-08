using System;
using Molder.Controllers;
using Molder.Extensions;
using Molder.Service.Helpers;
using Molder.Service.Infrastructures;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Molder.Service.Extension;

namespace Molder.Service.Models
{
    public class RequestDto
    {
        private readonly IEnumerable<Header> headers;
        private readonly VariableController variableController;

        public RequestDto(IEnumerable<Header> headers, VariableController variableController)
        {
            this.headers = headers;
            this.variableController = variableController;

            Header = SetData(HeaderType.HEADER);
            Query = SetData(HeaderType.QUERY);
            
            if (headers.CheckParameter(HeaderType.CREDENTIAL))
            {
                Credentials = GetCredentials();
            }

            if (headers.CheckParameter(HeaderType.TIMEOUT))
            {
                Timeout = GetTimeout();
            }

            if (headers.CheckParameter(HeaderType.BODY))
            {
                var body = GetBody();
                var obj = body.GetObject();
                Content = obj.GetHttpContent(body);
            }
        }

        public Dictionary<string, string> Header { get; private set; } = null;
        public Dictionary<string, string> Query { get; private set; } = null;
        public HttpContent Content { get; private set; } = null;
        public ICredentials Credentials { get; private set; } = null;
        public int? Timeout { get; set; } = null;

        private Dictionary<string, string> SetData(HeaderType headerType)
        {
            return headers
                .Where(h => h.Style == headerType)
                .ToDictionary(e => e.Name, e => this.variableController.ReplaceVariables(e.Value));
        }

        private int GetTimeout()
        {
            var headerValue = headers.FirstOrDefault(h => h.Style == HeaderType.TIMEOUT)?.Value;
            var value = variableController.ReplaceVariables(headerValue) ?? headerValue;

            return int.Parse(value);
        }

        private string GetBody()
        {
            var name = headers.FirstOrDefault(h => h.Style == HeaderType.BODY)?.Value;
            var value = variableController.GetVariableValueText(name);
            return value ?? name;
        }

        private ICredentials GetCredentials()
        {
            var name = headers.FirstOrDefault(h => h.Style == HeaderType.CREDENTIAL)?.Value;
            return variableController.GetVariableValue(name) as ICredentials ?? null;
        }
    }
}