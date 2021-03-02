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
            return Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute);
        }
    }
}
