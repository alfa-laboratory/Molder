using System;

namespace Molder.Service.Helpers
{
    public static class Validate
    {
        /// <summary>
        /// Валидация Url адреса формата (http:// )
        /// </summary>   
        public static bool ValidateUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }
    }
}
