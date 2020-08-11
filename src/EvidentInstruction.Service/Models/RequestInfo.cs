using System.ComponentModel.DataAnnotations;
using System.Net.Http;

namespace EvidentInstruction.Service.Models
{
    public class RequestInfo
    {
        [Required(ErrorMessage = "Url is required")]
        public string Url { get; set; }

        [Required(ErrorMessage = "HttpMethod is required")]
        public HttpMethod Method { get; set; }

        public string Name { get; set; } = null;
        public HttpContent Content { get; set; }
        public ServiceAttribute ServiceAttribute { get; set; }

    }
}