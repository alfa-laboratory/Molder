using System.Collections.Generic;
using System.Linq;
using Molder.Service.Infrastructures;
using Molder.Service.Models;

namespace Molder.Service.Extension
{
    public static class HeaderExtensions
    {
        public static bool CheckParameter(this IEnumerable<Header> headers, HeaderType parameter)
        {
            return headers.Count(h => h.Style == parameter) == 1;
        }
    }
}