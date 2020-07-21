using System.ComponentModel.DataAnnotations;

namespace EvidentInstruction.Tests.Models
{
    public class UrlModel
    {
        [Required(ErrorMessage = "Url is required")]
        public string Url { get; set; }
    }
}
