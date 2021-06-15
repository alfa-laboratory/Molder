using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Molder.Helpers
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

        public static bool TryParseToXml(this object obj)
        {
            try
            {
                XDocument.Parse(obj.ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}