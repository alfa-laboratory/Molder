using Molder.Controllers;
using Molder.Extensions;
using Molder.Service.Helpers;
using Molder.Service.Infrastructures;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

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

            var body = GetBody();
            if (body != null)
            {
                var obj = body.GetObject();
                Content = obj.GetHttpContent(body);
            }
        }

        public Dictionary<string, string> Header { get; private set; } = null;
        public Dictionary<string, string> Query { get; private set; } = null;
        public HttpContent Content { get; private set; } = null;

        private Dictionary<string, string> SetData(HeaderType headerType)
        {
            return headers
                .Where(h => h.Style == headerType)
                .ToDictionary(e => e.Name, e => this.variableController.ReplaceVariables(e.Value));
        }

        private string GetBody()
        {
            var isCheck = headers.Count(h => h.Style == HeaderType.BODY);
            if(isCheck == 1)
            {
                var name = headers.FirstOrDefault(h => h.Style == HeaderType.BODY).Value;
                var value = variableController.GetVariableValueText(name);
                return value ?? name;
            }
            return null;
        }
    }
}