using EvidentInstruction.Controllers;
using EvidentInstruction.Service.Infrastructures;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace EvidentInstruction.Service.Models
{
    public class RequestDto
    {
        private readonly List<Header> headers;
        private readonly VariableController variableController;

        public RequestDto(List<Header> headers, VariableController variableController)
        {
            this.headers = headers;
            this.variableController = variableController;
        }

        public Dictionary<string, string> Header => GetDictionary(HeaderType.HEADER.ToString());
        public Dictionary<string, string> Body => GetDictionary(HeaderType.BODY.ToString());

        public Dictionary<string, string> Query => GetDictionary(HeaderType.QUERY.ToString());

        private Dictionary<string, string> GetDictionary(string header)
        {
            return headers.Where(x => x.Style.ToString().ToUpper().Equals(header))
                          .ToDictionary(head => head.Name, head => this.variableController.ReplaceVariables(head.Value));
        }

        public StringContent Content = new StringContent(string.Empty);
    }
}
