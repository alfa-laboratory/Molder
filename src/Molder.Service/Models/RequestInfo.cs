using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using Molder.Service.Infrastructures;

namespace Molder.Service.Models
{
    public class RequestInfo
    {
        [Required(ErrorMessage = "Url is required")]
        public string Url { get; set; }

        [Required(ErrorMessage = "Headers is required")]
        public Dictionary<string, string> Headers { get; set; }

        [Required(ErrorMessage = "HttpMethod is required")]
        public HttpMethod Method { get; set; } 

        [Required(ErrorMessage = "Content is required")]
        public HttpContent Content { get; set; } 

        public ICredentials Credential { get; set; }

        private int? _timeout = null;
        public int? Timeout
        {
            get => _timeout ?? Constants.DEFAULT_TIMEOUT;
            set => _timeout = value;
        }
    }
}