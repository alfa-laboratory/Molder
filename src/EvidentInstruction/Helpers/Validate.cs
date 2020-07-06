using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EvidentInstruction.Helpers
{
    public static class Validate
    {
        public static (bool isValid, ICollection<ValidationResult> results) ValidateModel(object obj)
        {
            ValidationContext vc = new ValidationContext(obj);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(obj, vc, results, true);
            return (isValid, results);
        }
    }
}