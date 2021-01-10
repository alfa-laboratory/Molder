using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;

namespace EvidentInstruction.Service.Models
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
    }
}