using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EvidentInstruction.Tests.Models
{
    [ExcludeFromCodeCoverage]
    public class UrlModel
    {
        [Required(ErrorMessage = "Url is required")]
        public string Url { get; set; }
    }
}
